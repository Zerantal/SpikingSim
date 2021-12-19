using System;

using MathLib.Statistics;

namespace SpikingLibrary
{
    public class NonLearningNoisySynapseFactory : ISynapseFactory
    {
        private readonly INumberGenerator _delayGenerator;
        private readonly INumberGenerator _weightGenerator;
        private readonly INumberGenerator _noiseSource;

        public NonLearningNoisySynapseFactory(INumberGenerator delayGenerators, INumberGenerator efficacyGenerator,
            INumberGenerator noiseSource)
        {
            // Contract.Requires(delayGenerators != null);
            // Contract.Requires(efficacyGenerator != null);
            // Contract.Requires(noiseSource != null);

            _delayGenerator = delayGenerators;
            _weightGenerator = efficacyGenerator;
            _noiseSource = noiseSource;
        }

        #region ISynapseFactory Members

        public Synapse CreateSynapse()
        {
            int delay = Convert.ToInt32(_delayGenerator.Number);

            if (delay < 1)
                delay = 1;

            return new NonLearningNoisySynapse(delay, _weightGenerator.Number, _noiseSource);            
        }

        #endregion

        #region IDeepCloneable<ISynapseFactory> Members

        public ISynapseFactory DeepClone()
        {
            return new NonLearningNoisySynapseFactory(_delayGenerator.DeepClone(), _weightGenerator.DeepClone(),
                                                      _noiseSource.DeepClone());
        }

        #endregion

    }
}
