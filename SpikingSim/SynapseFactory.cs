using System;

using MathLib.Statistics;

namespace SpikingLibrary
{
    public class SynapseFactory : ISynapseFactory
    {
        private readonly INumberGenerator _delayGenerator;
        private readonly INumberGenerator _weightGenerator;
        private readonly StdpParameters _stdpParams;

        public SynapseFactory(INumberGenerator delayGenerators, INumberGenerator efficacyGenerator)
        {
            // Contract.Requires(delayGenerators != null);
            // Contract.Requires(efficacyGenerator != null);

            _delayGenerator = delayGenerators;
            _weightGenerator = efficacyGenerator;
            _stdpParams = StdpParameters.RatVisualCortexL23;
        }

        public SynapseFactory(INumberGenerator delayGenerators, INumberGenerator efficacyGenerator, StdpParameters stdpParameters)
        {
            // Contract.Requires(delayGenerators != null);
            // Contract.Requires(efficacyGenerator != null);

            _delayGenerator = delayGenerators;
            _weightGenerator = efficacyGenerator;
            _stdpParams = stdpParameters;
        }

        #region ISynapseFactory Members

        public Synapse CreateSynapse()
        {
            int delay = Convert.ToInt32(_delayGenerator.Number);

            if (delay < 1)
                delay = 1;

            return new Synapse(delay, _weightGenerator.Number, _stdpParams);           
        }

        #endregion


        #region IDeepCloneable<ISynapseFactory> Members

        public ISynapseFactory DeepClone()
        {
            return new SynapseFactory(_delayGenerator.DeepClone(), _weightGenerator.DeepClone(), _stdpParams);                        
        }

        #endregion
    }
}
