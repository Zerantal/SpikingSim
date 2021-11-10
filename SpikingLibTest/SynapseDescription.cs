using System.Diagnostics.Contracts;

using SpikingLibrary;

namespace SpikingLibTest
{
    internal class SynapseDescription
    {
        public StdpParameters LearningParameters { get; set; }
        public int AxonalDelay { get; set; }

        public SynapseDescription(int axonalDelay, StdpParameters learningParams)
        {
            // Contract.Requires(axonalDelay > 0);
            // Contract.Requires(learningParams != null);

            AxonalDelay = axonalDelay;
            LearningParameters = learningParams;
        }

        public void SetParameters(int axonalDelay, StdpParameters learningParams)
        {
            // Contract.Requires(axonalDelay >= 1);
            // Contract.Requires(learningParams != null);

            AxonalDelay = axonalDelay;
            LearningParameters = learningParams;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(LearningParameters != null);
            Contract.Invariant(AxonalDelay > 0);
        }
    }
}
