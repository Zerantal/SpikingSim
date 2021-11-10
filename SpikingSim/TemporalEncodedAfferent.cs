using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System;

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
                foreach (Tuple<Neuron, Synapse> t in _pendingNeuronConnections)
                {
                    Neuron postSynapticNeuron = t.Item1;
                    Synapse syn = t.Item2;
                    Contract.Assume(postSynapticNeuron != null);
                    syn.PostsynapticNeuron = postSynapticNeuron;
                    Axon.Add(syn);
                    postSynapticNeuron.AddDendriticSynapse(syn);
                }
                _pendingNeuronConnections.Clear();
                _neuronConnectionEventScheduled = false;
            }
        }

        public override void Stimulate(double value)
        {
            int timeInterval = (int) (value/_range*_timeSlice);
            if (timeInterval <= 0)      // clamp signel range
                timeInterval = 1;
            if (timeInterval > _timeSlice)
                timeInterval = _timeSlice;
            
            SpikingNetEngine.Scheduler.ScheduleEvent(new ScheduledEvent(Sched_signal), timeInterval);            
        }

        private void Sched_signal(long time)
        {
            foreach (Synapse s in Axon)
                s.ActivateSynapse(time);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_min < _max);
            Contract.Invariant(_timeSlice >= 10);            
        }
    }
}
