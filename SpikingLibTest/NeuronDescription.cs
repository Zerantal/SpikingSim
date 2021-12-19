using SpikingLibrary;

namespace SpikingLibTest
{
    internal class NeuronDescription
    {
        public NeuronDescription(NeuronParameters parameters)
        {
            // Contract.Requires(parameters != null);
            Parameters = parameters;
        }

        public NeuronParameters Parameters { get; set; }

    }
}
