using System;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;

namespace SpikingLibrary
{
    public class NeuronProbe
    {        
        private volatile bool _started;
        private volatile int _updateTime;
        private volatile bool _stopping;

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
            _onNeuronProbedDelegate = ReportNeuronState;
            _asyncOp = AsyncOperationManager.CreateOperation(null);
            _pendingClassUpdate = null;            
        }

        public int UpdateInterval
        {
            get => _updateTime;
            set => _updateTime = value;
        }

        public Neuron Target { get; set; }

        public void Start()
        {
            if (_started) return;

            SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_ProbeNeuron, _updateTime);
            _started = true;
            _stopping = false;
        }

        public void Stop()
        {
            _stopping = true;
        }


        // callback used by scheduler. i.e., this method is executed 
        // on the neural network thread
        private void Scheduler_ProbeNeuron(long time)
        {
            // Contract.Requires(time >= 1);

            double u = Target.U;
            double v = Target.V;

            NeuronProbeUpdateEventArgs e = new NeuronProbeUpdateEventArgs(time, u, v);

            _asyncOp.Post(_onNeuronProbedDelegate, e);

            if (!_stopping)
                SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_ProbeNeuron, _updateTime);
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
            NeuronProbed?.Invoke(this, e);
        }

        public void ChangeNeuronType(NeuronParameters neuronType)
        {
            // Contract.Requires(neuronType != null);

            if (!SpikingNetEngine.IsRunning)
                Target.NeuronType = neuronType;
            else
            {
                lock (_syncObj)
                {
                    _pendingClassUpdate = neuronType;
                    if (_updatesFlag.HasFlag(PendingUpdates.ClassUpdateScheduled)) return;
                    _updatesFlag |= PendingUpdates.ClassUpdateScheduled;
                    SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_ChangeClass, 1);
                }
            }
        }
        
        private void Scheduler_ChangeClass(long time)
        {
            lock (_syncObj)
            {
                if (_pendingClassUpdate != null)
                    Target.NeuronType = _pendingClassUpdate;
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
                Target.V = v;
            else
            {
                lock (_syncObj)
                {
                    _pendingVChange = v;
                    if (_updatesFlag.HasFlag(PendingUpdates.VChangeScheduled)) return;

                    _updatesFlag |= PendingUpdates.VChangeScheduled;
                    SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_ChangeV, 1);
                }
            }
        }        

        private void Scheduler_ChangeV(long time)
        {
            lock (_syncObj)
            {                
                Target.V = _pendingVChange;
                _updatesFlag &= ~PendingUpdates.VChangeScheduled;
            }
        }

        public void ChangeU(double u)
        {
            if (!SpikingNetEngine.Scheduler.IsRunning)
                Target.U = u;
            else
            {
                lock (_syncObj)
                {
                    _pendingUChange = u;
                    if (_updatesFlag.HasFlag(PendingUpdates.UChangeScheduled)) return;

                    _updatesFlag |= PendingUpdates.UChangeScheduled;
                    SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_ChangeU, 1);
                }
            }    
        }        

        private void Scheduler_ChangeU(long time)
        {
            lock (_syncObj)
            {
                Target.U = _pendingUChange;
                _updatesFlag &= ~PendingUpdates.UChangeScheduled;
            }            
        }

        public void IncrementState(double uIncrement, double vIncrement)
        {
            if (!SpikingNetEngine.Scheduler.IsRunning)
            {
                Target.U += uIncrement;
                Target.V += vIncrement;
            }
            else
            {
                lock (_syncObj)
                {
                    _pendingUIncrement = uIncrement;
                    _pendingVIncrement = vIncrement;
                    if (_updatesFlag.HasFlag(PendingUpdates.IncrementScheduled)) return;

                    _updatesFlag |= PendingUpdates.IncrementScheduled;
                    SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_IncrementState, 1);
                }
            }            
        }

        private void Scheduler_IncrementState(long time)
        {
            lock (_syncObj)
            {
                Target.U += _pendingUIncrement;
                Target.V += _pendingVIncrement;                
                _updatesFlag &= ~PendingUpdates.IncrementScheduled;
            }
        }

    }
}
