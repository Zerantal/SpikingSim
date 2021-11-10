using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Collections.ObjectModel;

namespace SpikingLibrary
{
    public abstract class AfferentNeuron
    {
        private readonly List<Synapse> _axon = new List<Synapse>();        

        protected internal Collection<Synapse> Axon
        {
            get { return new Collection<Synapse> (_axon); }            
        }

        public abstract void Stimulate(double value);

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(Axon != null);
        }
    }
}
