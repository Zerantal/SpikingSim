using System.Diagnostics.Contracts;
using System;

using MathLib.Statistics;

namespace SpikingLibrary
{
    public class NonLearningSynapseFactory : ISynapseFactory
    {
        private readonly INumberGenerator _delayGenerator;
        private readonly INumberGenerator _weightGenerator;

        public NonLearningSynapseFactory(INumberGenerator delayGenerators, INumberGenerator efficacyGenerator)
        {
            // Contract.Requires(delayGenerators != null);
            // Contract.Requires(efficacyGenerator != null);

            _delayGenerator = delayGenerators;
            _weightGenerator = efficacyGenerator;           
        }
        #region ISynapseFactory Members

        public Synapse CreateSynapse()
        {          
            int delay = Convert.ToInt32(_delayGenerator.Number);

            if (delay < 1)
                delay = 1;
     
            return new NonLearningSynapse(delay, _weightGenerator.Number);
        }

        #endregion

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_delayGenerator != null);
            Contract.Invariant(_weightGenerator != null);
        }

        #region IDeepCloneable<ISynapseFactory> Members

        public ISynapseFactory DeepClone()
        {
            return new NonLearningSynapseFactory(_delayGenerator.DeepClone(), _weightGenerator.DeepClone());            
        }

        #endregion
    }
}
