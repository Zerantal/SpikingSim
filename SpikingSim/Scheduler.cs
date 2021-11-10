#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading;

#endregion

namespace SpikingLibrary
{
    public delegate void ScheduledEvent(long time);

    /// <summary>
    ///   Class for scheduling the firing of events
    /// </summary>
    /// <remarks>
    ///   The maximum queue priority that an event can have is 4026531840
    /// </remarks>
    public sealed class Scheduler : IDisposable
    {
        private readonly int _numberOfEventBins;
        private readonly List<ScheduledEventItem> _pendingEventAdditions;
        private readonly List<ScheduledEvent>[] _schedulingBins;

        private readonly EventWaitHandle _waitHandle;
        private int _currentEventBin;
        private long _currentTime;

        private int _numEvents;
        private List<Tuple<int, ScheduledEvent>> _overflowBuffer;
        private volatile bool _paused; // false by default      
        private volatile bool _running; // false by default
        private volatile bool _schedulingCancelled;
        private volatile bool _threadSuspended; // true if no events scheduled or scheduler is paused      
        private int _executingThreadId;

        public Scheduler(int maxEventBins)
        {
            // Contract.Requires(maxEventBins >= 1);

            _numberOfEventBins = maxEventBins;

            _schedulingBins = new List<ScheduledEvent>[_numberOfEventBins];
            _overflowBuffer = new List<Tuple<int, ScheduledEvent>>();

            for (int i = 0; i < _numberOfEventBins; i++)
                _schedulingBins[i] = new List<ScheduledEvent>();

            _pendingEventAdditions = new List<ScheduledEventItem>();
            _waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            _schedulingCancelled = false;
        }

        // Thread safe
        public bool IsRunning
        {
            get
            {
                return _running;                
            }
        }

        #region IDisposable Members

        #endregion

        // thread safe
        public void ScheduleEvent(Collection<ScheduledEventItem> eventList)
        {
            // Contract.Requires(eventList != null);

            if (Thread.CurrentThread.ManagedThreadId == _executingThreadId)
            {
                foreach (ScheduledEventItem ev in eventList)
                    AddEvent(ev.ScheduledEvent, ev.TimeInterval);
            }
            else
            {
                lock (_pendingEventAdditions)
                {
                    foreach (ScheduledEventItem ev in eventList)
                        _pendingEventAdditions.Add(ev);

                    if (_threadSuspended && !_paused) 
                        _waitHandle.Set();
                }


            }
        }

        // Thread safe
        public void ScheduleEvent(ScheduledEvent scheduledEvent, int timeInterval)
        {
            // Contract.Requires(scheduledEvent != null);
            // Contract.Requires(timeInterval > 0);

            if (Thread.CurrentThread.ManagedThreadId == _executingThreadId)
            {
                                AddEvent(scheduledEvent, timeInterval);
            }
            else                        
            {
                lock (_pendingEventAdditions)
                {
                    _pendingEventAdditions.Add(new ScheduledEventItem(scheduledEvent, timeInterval));
                    
                    if (_threadSuspended && !_paused)
                        _waitHandle.Set();
                }

            }          
        }

        private void AddEvent(ScheduledEvent scheduledEvent, int time)
        {
            // Contract.Requires(scheduledEvent != null);
            // Contract.Requires(time >= 1);

            _numEvents++;
            if (time >= _numberOfEventBins)
            {
                _overflowBuffer.Add(new Tuple<int, ScheduledEvent>(time + _currentEventBin , scheduledEvent));
            }
            else
            {
                int eventBin = _currentEventBin + time;

                if (eventBin >= _numberOfEventBins)
                    eventBin -= _numberOfEventBins;

                Contract.Assume(eventBin >= 0 && eventBin < _numberOfEventBins);

                ((IList) _schedulingBins[eventBin]).Add(scheduledEvent);
            }
        }

        public void StartAsync(string threadName)
        {
            SchedulerThreadDelegate schedulerTask = ExecuteScheduler;

            if (_running)
                throw new InvalidOperationException("The event scheduler is currently running.");

            _running = true;

            AsyncOperation async = AsyncOperationManager.CreateOperation(null);
            schedulerTask.BeginInvoke(threadName, null, async);
        }

        private void ExecuteScheduler(string threadName)
        {
            // Contract.Requires(threadName != null);

            Thread.CurrentThread.Name = threadName;
            Thread.VolatileWrite(ref _executingThreadId, Thread.CurrentThread.ManagedThreadId);
            
            while (!_schedulingCancelled)
            {                    
                // If new event schedulings are pending, add them to tree
                lock (_pendingEventAdditions)
                {
                    if (_pendingEventAdditions.Count > 0)
                        AddPendingEvents();
                }

                foreach (ScheduledEvent ev in _schedulingBins[_currentEventBin])
                    ev.Invoke(_currentTime);
                _numEvents -= _schedulingBins[_currentEventBin].Count;
                _schedulingBins[_currentEventBin].Clear();
                _currentTime++;
                _currentEventBin++;

                if (_currentEventBin == _numberOfEventBins)
                {
                    _currentEventBin = 0;
                    // fold _overflowBuffer events into main event array
                    if (_overflowBuffer.Count != 0)
                    {
                        Tuple<int, ScheduledEvent>[] tmp = _overflowBuffer.ToArray();
                        _numEvents -= _overflowBuffer.Count;
                        _overflowBuffer = new List<Tuple<int, ScheduledEvent>>();
                        foreach (Tuple<int, ScheduledEvent> ev in tmp)
                            AddEvent(ev.Item2, ev.Item1 - _numberOfEventBins);
                    }
                }


                if (_numEvents != 0 && !_paused) continue;
                 _threadSuspended = true;
                _waitHandle.WaitOne();
                _threadSuspended = false;
            }


                PurgeScheduler();                     
        }

        // Purge all events from scheduler and leave scheduler in a clean state
        // so that is may be restarted anew
        private void PurgeScheduler()
        {
            lock (_pendingEventAdditions)
            {
                _pendingEventAdditions.Clear();
                for (int i = 0; i < _numberOfEventBins; i++)
                    _schedulingBins[i].Clear();
            }

                _currentEventBin = 0;
                //_currentTime = 0;
                _numEvents = 0;
                _overflowBuffer.Clear();
                _running = false;  
                _paused = false;
                _schedulingCancelled = false;
                _threadSuspended = false;          
        }

        public void Stop()
        {
            _schedulingCancelled = true;
            _paused = false;

            if (_threadSuspended)
                _waitHandle.Set();

        }

        // thread safe
        public void Pause()
        {

            if (_running)
            {
                _paused = true;
            }

        }

        // Thread safe
        public void Resume()
        {
            if (_paused)
                _waitHandle.Set();
            _paused = false;

        }

        private void AddPendingEvents()
        {
            foreach (ScheduledEventItem ev in _pendingEventAdditions)
            {
                AddEvent(ev.ScheduledEvent, ev.TimeInterval);
            }
            _pendingEventAdditions.Clear();
        }
       
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_pendingEventAdditions != null);
            Contract.Invariant(_waitHandle != null);
            Contract.Invariant(_currentTime >= 0);
            Contract.Invariant(_schedulingBins != null);
            Contract.Invariant(_schedulingBins.Length == _numberOfEventBins);
            Contract.Invariant(_overflowBuffer != null);
        }

        #region Nested type: SchedulerThreadDelegate

        private delegate void SchedulerThreadDelegate(string name);

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _waitHandle.Dispose();            
        }

        #endregion
    }
}