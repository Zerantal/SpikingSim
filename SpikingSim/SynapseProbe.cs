using System;
using System.ComponentModel;
using System.Threading;

namespace SpikingLibrary
{
    public class SynapseProbe
    {
        private volatile bool _started;       
        private volatile int _updateTime;
        private volatile bool _stopping;
        private volatile Synapse _target;

        public event EventHandler<SynapseProbeUpdateEventArgs> SynapseProbed;
        private readonly SendOrPostCallback _onSynapseProbedDelegate;

        private readonly AsyncOperation _asyncOp;
        private readonly object _syncObj = new object();
        
        private  int _pendingAxonalDelayUpdate = 1;
        private  bool _axonalDelayUpdateScheduled;
        private double _pendingWeightUpdate;
        private bool _weightUpdateScheduled;

        private double _weightValue;
        private volatile bool _weightSuccessfullyRetrieved;

        public SynapseProbe(Synapse target, int updateInterval = int.MaxValue)
        {
            // Contract.Requires(target != null);
            // Contract.Requires(updateInterval > 0);

            Target = target;
            UpdateInterval = updateInterval;            
            _onSynapseProbedDelegate = ReportSynapseState;
            _asyncOp = AsyncOperationManager.CreateOperation(null);
        }

        public int UpdateInterval
        {
            get => _updateTime;
            set => _updateTime = value;
        }

        public Synapse Target
        {
            get => _target;
            set =>
                // Contract.Requires(value != null);
                _target = value;
        }

        public void Start()
        {
            if (_started) return;

            SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_ProbeSynapse, _updateTime);
            _started = true;
            _stopping = false;
        }

        public void Stop()
        {
            _stopping = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public double GetWeight()
        {
            _weightSuccessfullyRetrieved = false;
            SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_GetWeight, 1);
            while (!_weightSuccessfullyRetrieved)
            {
                Thread.Sleep(0);
            }
            
            return _weightValue;            
        }

        private void Scheduler_GetWeight(long time)
        {
            _weightValue = _target.Weight;
            _weightSuccessfullyRetrieved = true;
        }

        public void SetWeight(double weight)
        {           
            if (!SpikingNetEngine.IsRunning)
                _target.Weight = weight;
            else
            {
                lock (_syncObj)
                {
                    _pendingWeightUpdate = weight;
                    if (_weightUpdateScheduled) return;

                    _weightUpdateScheduled = true;
                    SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_SetWeight, 1);
                }
            }
        }

        private void Scheduler_SetWeight(long time)
        {
            lock (_syncObj)
            {
                _target.Weight = _pendingWeightUpdate;
                _weightUpdateScheduled = false;
            }
        }

        public void SetAxonalDelay(int time)
        {
            // Contract.Requires(time >= 1);

            if (!SpikingNetEngine.IsRunning)
                _target.AxonalDelay = time;
            else
            {
                lock (_syncObj)
                {
                    _pendingAxonalDelayUpdate = time;
                    if (_axonalDelayUpdateScheduled) return;

                    _axonalDelayUpdateScheduled = true;
                    SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_SetAxonalDelay, 1);
                }
            }
        }

        private void Scheduler_SetAxonalDelay(long time)
        {
            lock (_syncObj)
            {
                _target.AxonalDelay = _pendingAxonalDelayUpdate;
                _axonalDelayUpdateScheduled = false;                
            }
        }

        // callback used by scheduler. i.e., this method is executed 
        // on the neural network thread
        private void Scheduler_ProbeSynapse(long time)
        {
            // Contract.Requires(time >= 1);

            double efficacy = _target.Weight;

            SynapseProbeUpdateEventArgs e = new SynapseProbeUpdateEventArgs(time, efficacy);

            _asyncOp.Post(_onSynapseProbedDelegate, e);

            if (!_stopping)
                SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_ProbeSynapse, _updateTime);
        }

        // This method is invoked via the AsyncOperation object,
        // so it is guaranteed to be executed on the correct thread.
        private void ReportSynapseState(object state)
        {
            SynapseProbeUpdateEventArgs e = state as SynapseProbeUpdateEventArgs;

            OnSynapseProbed(e);
        }

        protected void OnSynapseProbed(SynapseProbeUpdateEventArgs e)
        {
            SynapseProbed?.Invoke(this, e);
        }
    }
}
