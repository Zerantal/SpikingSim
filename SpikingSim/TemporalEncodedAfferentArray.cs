using System.Diagnostics.Contracts;
using System;
using System.Collections.Generic;

namespace SpikingLibrary
{
    // ReSharper disable once UnusedMember.Global
    public class TemporalEncodedAfferentArray
    {
        private readonly TemporalEncodedAfferent[] _afferentArray;

        private readonly object _syncObj = new object();
        private readonly List<Tuple<int, Neuron, Synapse>> _pendingNeuronConnections;
        private bool _neuronConnectionEventScheduled;

        public TemporalEncodedAfferentArray(double minValue, double maxValue, int timeSlice, int arraySize)
        {
            // Contract.Requires(minValue < maxValue);
            // Contract.Requires(timeSlice >= 10);
            // Contract.Requires(arraySize > 0);

            ArraySize = arraySize;
            _afferentArray = new TemporalEncodedAfferent[arraySize];
            for (int i = 0; i < _afferentArray.Length; i++)            
                _afferentArray[i] = new TemporalEncodedAfferent(minValue, maxValue, timeSlice);

            _pendingNeuronConnections = new List<Tuple<int, Neuron, Synapse>>();  

        }

        public int ArraySize { get; }

        // ReSharper disable once UnusedMember.Global
        public void ConnectNeuronTo(int presynapticNeuronIndex, Neuron postsynapticNeuron, Synapse synapse)
        {
            // Contract.Requires(postsynapticNeuron != null);
            // Contract.Requires(synapse != null);
            // Contract.Requires(presynapticNeuronIndex > 0 && presynapticNeuronIndex < ArraySize);

            // prevent possible race condition
            // ReSharper disable once EmptyEmbeddedStatement
            while (SpikingNetEngine.IsStopping && SpikingNetEngine.IsRunning) ;

            if (!SpikingNetEngine.IsRunning)
            {
                _afferentArray[presynapticNeuronIndex].Axon.Add(synapse);
                synapse.PostsynapticNeuron = postsynapticNeuron;
                postsynapticNeuron.AddDendriticSynapse(synapse);
            }
            else
            {
                lock (_syncObj)
                {
                    _pendingNeuronConnections.Add(new Tuple<int, Neuron, Synapse>(presynapticNeuronIndex, postsynapticNeuron, synapse));
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
                foreach (var (presynapticNeuronIdx, postSynapticNeuron, synapse) in _pendingNeuronConnections)
                {
                    Contract.Assume(postSynapticNeuron != null);
                    synapse.PostsynapticNeuron = postSynapticNeuron;
                    _afferentArray[presynapticNeuronIdx].Axon.Add(synapse);
                    postSynapticNeuron.AddDendriticSynapse(synapse);
                }
                _pendingNeuronConnections.Clear();
                _neuronConnectionEventScheduled = false;
            }
        }
    }
}
