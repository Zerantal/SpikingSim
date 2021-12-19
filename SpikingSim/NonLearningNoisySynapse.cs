using MathLib.Statistics;

namespace SpikingLibrary
{
    public class NonLearningNoisySynapse : Synapse
    {
        private readonly INumberGenerator _noiseSource;

        public NonLearningNoisySynapse(int delay, double efficacy,
            INumberGenerator noiseSource) : base(delay, efficacy, StdpParameters.HippocampalCulture)
        {            
            // Contract.Requires(noiseSource != null);
            // Contract.Requires(delay >= 1);

            _noiseSource = noiseSource;
        }

        internal override void ActivateSynapse(long time)
        {            
            PostsynapticNeuron.V += Weight + _noiseSource.Number;              
        }

        internal override void Bap(long time)
        {
        }

    }
}
