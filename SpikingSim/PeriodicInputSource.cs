using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Threading;

namespace SpikingLibrary
{    
    public class PeriodicInputSource : InputSource
    {                  
        private volatile bool _stopping;      // false by default
        private volatile bool _running;       // false by default
        private int[] _spikeTimings;      // 1 ms by default        
        private volatile int _maxInputCycles;
        private readonly List<Synapse> _axon;
        private int _currentInjectionCycleCount;     // doesn't need to be locked

        private readonly object _syncObj = new object();
        private readonly List<Tuple<NeuronCollection, ISynapseFactory>> _pendingBulkConnections;
        private readonly List<Tuple<Neuron, Synapse>> _pendingNeuronConnections;
        private int[] _newSpikeTimings = {10};
        private bool _pendingSpikeTimingChange;
        private bool _bulkConnectionEventScheduled;
        private bool _neuronConnectionEventScheduled;

        public event EventHandler  PeriodicInputFinished;
        private readonly SendOrPostCallback _onInputFinishedDelegate;
        private readonly AsyncOperation _asyncOp;
        private  int _spikeIdx;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public PeriodicInputSource(int periodicity = 10000, int maxInputCycles = int.MaxValue)
        {
            // Contract.Requires(periodicity >= 1);
            // Contract.Requires(maxInputCycles >= 1);

            _spikeTimings = new[] { periodicity };            
            _maxInputCycles = maxInputCycles;
            _axon = new List<Synapse>();
            _pendingBulkConnections = new List<Tuple<NeuronCollection, ISynapseFactory>>();
            _pendingNeuronConnections = new List<Tuple<Neuron, Synapse>>();            
            
            _onInputFinishedDelegate = ReportInputFinished;
            _asyncOp = AsyncOperationManager.CreateOperation(null);
        }        

        public void SetSpikeTimings(int [] timings)
        {
            // Contract.Requires(timings != null);            
            // Contract.Requires(timings.Length > 0);
            // Contract.Requires(Contract.ForAll(timings, t => t > 0));            

            if (!_running)
                _spikeTimings = timings;
            else
            {
                lock (_syncObj)
                {
                    _newSpikeTimings = (int[])timings.Clone();

                    if (_pendingSpikeTimingChange) return;
                    _pendingSpikeTimingChange = true;                        
                    SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_ChangeSpikeTiming, 1);
                }
            }
        }        

        private void Scheduler_ChangeSpikeTiming(long time)
        {
            lock (_syncObj)
            {
                _spikeTimings = _newSpikeTimings;

                _pendingSpikeTimingChange = false;                                
            }
        }
        
        public void ConnectTo(NeuronCollection neuronGroup, ISynapseFactory synapseFactory)
        {
            // Contract.Requires(neuronGroup != null);
            // Contract.Requires(synapseFactory != null);
            // Contract.Requires(Contract.ForAll(neuronGroup, n => n != null));

            // prevent possible race condition
            while (SpikingNetEngine.IsStopping && SpikingNetEngine.IsRunning)
            {
            }

            if (!SpikingNetEngine.IsRunning)
            {
                foreach (Neuron postSynapticNeuron in neuronGroup)
                {
                    Contract.Assume(postSynapticNeuron != null);
                    Synapse syn = synapseFactory.CreateSynapse();
                    syn.PostsynapticNeuron = postSynapticNeuron;
                    _axon.Add(syn);
                    postSynapticNeuron.AddDendriticSynapse(syn);
                }
            }
            else
            {
                lock (_syncObj)
                {
                    _pendingBulkConnections.Add(new Tuple<NeuronCollection, ISynapseFactory>(
                                               neuronGroup.ShallowClone(), synapseFactory.DeepClone()));
                    if (_bulkConnectionEventScheduled) return;

                    _bulkConnectionEventScheduled = true;
                    SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_ConnectionEvent, 1);
                }
            }
        }
        
        private void Scheduler_ConnectionEvent(long time)
        {
            lock (_syncObj)
            {
                foreach (var (neuralNetwork, synapseFactory) in _pendingBulkConnections)
                {
                    foreach (Neuron postSynapticNeuron in neuralNetwork)
                    {
                        Contract.Assume(postSynapticNeuron != null);
                        Synapse syn = synapseFactory.CreateSynapse();
                        syn.PostsynapticNeuron = postSynapticNeuron;
                        _axon.Add(syn);
                        postSynapticNeuron.AddDendriticSynapse(syn);

                    }
                }
                _pendingBulkConnections.Clear();
                _bulkConnectionEventScheduled = false;
            }
        }
        
        public void ConnectTo(Neuron postsynapticNeuron, Synapse synapse)
        {            
            // Contract.Requires(synapse != null);
            // Contract.Requires(postsynapticNeuron != null);

            // prevent possible race condition
            while (SpikingNetEngine.IsStopping && SpikingNetEngine.IsRunning)
            {
            }

            if (!SpikingNetEngine.IsRunning)
            {
                _axon.Add(synapse);
                synapse.PostsynapticNeuron = postsynapticNeuron;
                postsynapticNeuron.AddDendriticSynapse(synapse);
            }        
            else
            {
                lock (_syncObj)
                {
                    _pendingNeuronConnections.Add(new Tuple<Neuron, Synapse>(postsynapticNeuron, synapse));
                    if (_neuronConnectionEventScheduled) return;

                    _neuronConnectionEventScheduled = true;
                    SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_SynapseConnectionEvent, 1);
                }
            }
        }
        
        private void Scheduler_SynapseConnectionEvent(long time)
        {
            lock (_syncObj)
            {
                foreach (var (postSynapticNeuron, synapse) in _pendingNeuronConnections)
                {
                    synapse.PostsynapticNeuron = postSynapticNeuron;
                    _axon.Add(synapse);
                    postSynapticNeuron.AddDendriticSynapse(synapse);                    
                }
                _pendingNeuronConnections.Clear();
                _neuronConnectionEventScheduled = false;
            }
        }

        public int MaxInputCycles
        {
            get => _maxInputCycles;
            set => _maxInputCycles = value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public override void Start(int startTime = 1)
        {
            if (!_running)
            {
                _running = true;
                _currentInjectionCycleCount = 0;
                SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_InjectSpike, startTime);
            }

            _stopping = false;
        }

        public override void Stop()
        {
            _stopping = true;
            _running = false;
        }

        private void Scheduler_InjectSpike(long time)
        {
            if (_stopping) return;

            foreach (Synapse s in _axon)
            {
                SpikingNetEngine.Scheduler.ScheduleEvent(s.ActivateSynapse, s.AxonalDelay);
            }
            
            if (_spikeIdx >= _spikeTimings.Length)
            {
                _spikeIdx = 0;
                _currentInjectionCycleCount++;
            }
            if (_currentInjectionCycleCount < _maxInputCycles)
            {
                SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_InjectSpike, _spikeTimings[_spikeIdx]);
                _spikeIdx++;
            }
            else
            {
                _asyncOp.Post(_onInputFinishedDelegate, EventArgs.Empty);
                _running = false;
            }
        }

        // This method is invoked via the AsyncOperation object,
        // so it is guaranteed to be executed on the correct thread.
        private void ReportInputFinished(object state)
        {
            EventArgs e = state as EventArgs;

            OnInputFinished(e);
        }

        protected void OnInputFinished(EventArgs e)
        {
            PeriodicInputFinished?.Invoke(this, e);
        }
    }
}
