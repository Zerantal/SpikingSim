using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System;
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable

namespace SpikingLibrary
{
    public class TemporalEncodedAfferent : AfferentNeuron
    {
        private readonly double _min;
        private readonly double _max;
        private readonly int _timeSlice;

        private readonly double _range;

        private readonly object _syncObj = new object();
        private readonly List<Tuple<Neuron, Synapse>> _pendingNeuronConnections;
        private bool _neuronConnectionEventScheduled;

        public TemporalEncodedAfferent(double minValue, double maxValue, int timeSlice)
        {
            // Contract.Requires(minValue < maxValue);
            // Contract.Requires(timeSlice >= 10);

            _min = minValue;
            _max = maxValue;
            _timeSlice = timeSlice;
            _range = _max - _min;

            _pendingNeuronConnections = new List<Tuple<Neuron, Synapse>>();   
        }

        public void ConnectTo(Neuron postsynapticNeuron, Synapse synapse)
        {
            // Contract.Requires(postsynapticNeuron != null);
            // Contract.Requires(synapse != null);

            // prevent possible race condition
            // ReSharper disable once EmptyEmbeddedStatement
            while (SpikingNetEngine.IsStopping && SpikingNetEngine.IsRunning) ;

            if (!SpikingNetEngine.IsRunning)
            {
                Axon.Add(synapse);
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
                    Contract.Assume(postSynapticNeuron != null);
                    synapse.PostsynapticNeuron = postSynapticNeuron;
                    Axon.Add(synapse);
                    postSynapticNeuron.AddDendriticSynapse(synapse);
                }
                _pendingNeuronConnections.Clear();
                _neuronConnectionEventScheduled = false;
            }
        }

        public override void Stimulate(double value)
        {
            int timeInterval = (int) (value/_range*_timeSlice);
            if (timeInterval <= 0)      // clamp signal range
                timeInterval = 1;
            if (timeInterval > _timeSlice)
                timeInterval = _timeSlice;
            
            SpikingNetEngine.Scheduler.ScheduleEvent(Scheduler_signal, timeInterval);            
        }

        private void Scheduler_signal(long time)
        {
            foreach (var s in Axon)
                s.ActivateSynapse(time);
        }
    }
}
