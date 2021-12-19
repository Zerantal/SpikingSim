using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SpikingLibrary
{
    public abstract class AfferentNeuron
    {
        private readonly List<Synapse> _axon = new List<Synapse>();        

        protected internal Collection<Synapse> Axon => new Collection<Synapse> (_axon);

        // ReSharper disable once UnusedMember.Global
        public abstract void Stimulate(double value);
    }
}
