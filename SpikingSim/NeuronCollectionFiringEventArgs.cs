using System;

namespace SpikingLibrary
{
    public class NeuronCollectionFiringEventArgs : EventArgs
    {
        public NeuronCollectionFiringEventArgs(Neuron firingNeuron, long time)
        {
            FiringNeuron = firingNeuron;
            Time = time;
        }

        public long Time { get; private set; }

        public Neuron FiringNeuron { get; private set; }
    }
}
