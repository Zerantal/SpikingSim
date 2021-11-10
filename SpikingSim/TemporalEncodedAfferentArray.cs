using System.Diagnostics.Contracts;
using System;
using System.Collections.Generic;

namespace SpikingLibrary
{
    public class TemporalEncodedAfferentArray
    {
        private TemporalEncodedAfferent[] _afferentArray;
        private int _arraySize;

        private readonly object _syncObj = new object();
        private readonly List<Tuple<int, Neuron, Synapse>> _pendingNeuronConnections;
        private bool _neuronConnectionEventScheduled;

        public TemporalEncodedAfferentArray(double minValue, double maxValue, int timeSlice, int arraySize)
        {
            // Contract.Requires(minValue < maxValue);
            // Contract.Requires(timeSlice >= 10);
            // Contract.Requires(arraySize > 0);

            _arraySize = arraySize;
            _afferentArray = new TemporalEncodedAfferent[arraySize];
            for (int i = 0; i < _afferentArray.Length; i++)            
                _afferentArray[i] = new TemporalEncodedAfferent(minValue, maxValue, timeSlice);

            _pendingNeuronConnections = new List<Tuple<int, Neuron, Synapse>>();  

        }

        public int ArraySize
        {
            get { return _arraySize; }
        }

        public void ConnectNeuronTo(int presynapticNeuronIndex, Neuron postsynapticNeuron, Synapse synapse)
        {
            // Contract.Requires(postsynapticNeuron != null);
            // Contract.Requires(synapse != null);
            // Contract.Requires(presynapticNeuronIndex > 0 && presynapticNeuronIndex < ArraySize);

            // prevent possible race condition
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
                    if (!_neuronConnectionEventScheduled)
                    {
                        _neuronConnectionEventScheduled = true;
                        SpikingNetEngine.Scheduler.ScheduleEvent(new ScheduledEvent(Sched_SynapseConnectionEvent), 1);
                    }
                }
            }
        }

        private void Sched_SynapseConnectionEvent(long time)
        {
            lock (_syncObj)
            {
                foreach (Tuple<int, Neuron, Synapse> t in _pendingNeuronConnections)
                {
                    Neuron postSynapticNeuron = t.Item2;
                    Synapse syn = t.Item3;
                    Contract.Assume(postSynapticNeuron != null);
                    syn.PostsynapticNeuron = postSynapticNeuron;
                    _afferentArray[t.Item1].Axon.Add(syn);
                    postSynapticNeuron.AddDendriticSynapse(syn);
                }
                _pendingNeuronConnections.Clear();
                _neuronConnectionEventScheduled = false;
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_afferentArray != null);
            Contract.Invariant(Contract.ForAll(_afferentArray, n => n != null));
            Contract.Invariant(_afferentArray.Length == ArraySize);
        }
    }
}
