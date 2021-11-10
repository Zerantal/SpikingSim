using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Diagnostics;

namespace SpikingLibrary
{
    public class NeuronProbe
    {        
        private volatile bool _started;
        private volatile int _updateTime;
        private volatile bool _stopping;
        private Neuron _target;

        public event EventHandler<NeuronProbeUpdateEventArgs> NeuronProbed;
        private readonly SendOrPostCallback _onNeuronProbedDelegate;

        private readonly AsyncOperation _asyncOp;

        [Flags]
        private enum PendingUpdates
        {
            ClassUpdateScheduled = 0x01,
            UChangeScheduled = 0x02,
            VChangeScheduled = 0x04,
            IncrementScheduled = 0x08        
        }

        private NeuronParameters _pendingClassUpdate;
        private double _pendingVChange;
        private double _pendingUChange;
        private double _pendingVIncrement;
        private double _pendingUIncrement;
        private PendingUpdates _updatesFlag;
        private readonly object _syncObj = new object();
        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public NeuronProbe(Neuron target, int updateInterval = int.MaxValue)
        {
            // Contract.Requires(updateInterval > 0);
            // Contract.Requires(target != null);

            Target = target;
            UpdateInterval = updateInterval;
            _onNeuronProbedDelegate = new SendOrPostCallback(ReportNeuronState);
            _asyncOp = AsyncOperationManager.CreateOperation(null);
            _pendingClassUpdate = null;            
        }

        public int UpdateInterval
        {
            get { return _updateTime; }
            set
            {
                // Contract.Requires(value > 0);

                _updateTime = value;
            }
        }

        public Neuron Target
        {
            get { return _target; }
            set
            {
                // Contract.Requires(value != null);
                
                _target = value;
            }
        }

        public void Start()
        {
            if (_started) return;

            SpikingNetEngine.Scheduler.ScheduleEvent(Sched_ProbeNeuron, _updateTime);
            _started = true;
            _stopping = false;
        }

        public void Stop()
        {
            _stopping = true;
        }


        // callback used by scheduler. i.e., this method is executed 
        // on the neural network thread
        private void Sched_ProbeNeuron(long time)
        {
            // Contract.Requires(time >= 1);

            double u = _target.U;
            double v = _target.V;

            NeuronProbeUpdateEventArgs e = new NeuronProbeUpdateEventArgs(time, u, v);

            _asyncOp.Post(_onNeuronProbedDelegate, e);

            if (!_stopping)
                SpikingNetEngine.Scheduler.ScheduleEvent(Sched_ProbeNeuron, _updateTime);
        }

        // This method is invoked via the AsyncOperation object,
        // so it is guaranteed to be executed on the correct thread.
        private void ReportNeuronState(object state)
        {
            NeuronProbeUpdateEventArgs e = state as NeuronProbeUpdateEventArgs;

            OnNeuronProbed(e);
        }

        protected void OnNeuronProbed(NeuronProbeUpdateEventArgs e)
        {
            if (NeuronProbed != null)
            {
                NeuronProbed(this, e);
            }
        }

        public void ChangeNeuronType(NeuronParameters neuronType)
        {
            // Contract.Requires(neuronType != null);

            if (!SpikingNetEngine.IsRunning)
                _target.NeuronType = neuronType;
            else
            {
                lock (_syncObj)
                {
                    _pendingClassUpdate = neuronType;
                    if (!_updatesFlag.HasFlag(PendingUpdates.ClassUpdateScheduled))
                    {
                        _updatesFlag |= PendingUpdates.ClassUpdateScheduled;
                        SpikingNetEngine.Scheduler.ScheduleEvent(Sched_ChangeClass, 1);
                    }
                }
            }
        }
        
        private void Sched_ChangeClass(long time)
        {
            lock (_syncObj)
            {
                if (_pendingClassUpdate != null)
                    _target.NeuronType = _pendingClassUpdate;
                else
                {
                    Debug.WriteLine("_pendingClassUpdate shouldn't be null");
                }
                _pendingClassUpdate = null;

                _updatesFlag &= ~PendingUpdates.ClassUpdateScheduled;
            }

        }

        public void ChangeV(double v)
        {
            if (!SpikingNetEngine.Scheduler.IsRunning)
                _target.V = v;
            else
            {
                lock (_syncObj)
                {
                    _pendingVChange = v;
                    if (!_updatesFlag.HasFlag(PendingUpdates.VChangeScheduled))
                    {
                        _updatesFlag |= PendingUpdates.VChangeScheduled;
                        SpikingNetEngine.Scheduler.ScheduleEvent(Sched_ChangeV, 1);
                    }
                }
            }
        }        

        private void Sched_ChangeV(long time)
        {
            lock (_syncObj)
            {                
                _target.V = _pendingVChange;
                _updatesFlag &= ~PendingUpdates.VChangeScheduled;
            }
        }

        public void ChangeU(double u)
        {
            if (!SpikingNetEngine.Scheduler.IsRunning)
                _target.U = u;
            else
            {
                lock (_syncObj)
                {
                    _pendingUChange = u;
                    if (!_updatesFlag.HasFlag(PendingUpdates.UChangeScheduled))
                    {
                        _updatesFlag |= PendingUpdates.UChangeScheduled;
                        SpikingNetEngine.Scheduler.ScheduleEvent(Sched_ChangeU, 1);
                    }
                }
            }    
        }        

        private void Sched_ChangeU(long time)
        {
            lock (_syncObj)
            {
                _target.U = _pendingUChange;
                _updatesFlag &= ~PendingUpdates.UChangeScheduled;
            }            
        }

        public void IncrementState(double uInc, double vInc)
        {
            if (!SpikingNetEngine.Scheduler.IsRunning)
            {
                _target.U += uInc;
                _target.V += vInc;
            }
            else
            {
                lock (_syncObj)
                {
                    _pendingUIncrement = uInc;
                    _pendingVIncrement = vInc;
                    if (!_updatesFlag.HasFlag(PendingUpdates.IncrementScheduled))
                    {
                        _updatesFlag |= PendingUpdates.IncrementScheduled;
                        SpikingNetEngine.Scheduler.ScheduleEvent(Sched_IncrementState, 1);
                    }
                }
            }            
        }

        private void Sched_IncrementState(long time)
        {
            lock (_syncObj)
            {
                _target.U += _pendingUIncrement;
                _target.V += _pendingVIncrement;                
                _updatesFlag &= ~PendingUpdates.IncrementScheduled;
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_updateTime > 0);
            Contract.Invariant(_target != null);
            Contract.Invariant(_asyncOp != null);         
        }
    }
}
