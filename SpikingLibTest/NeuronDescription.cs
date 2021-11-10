using System.Diagnostics.Contracts;

using SpikingLibrary;

namespace SpikingLibTest
{
    internal class NeuronDescription
    {
        private NeuronParameters _parameters;

        public NeuronDescription(NeuronParameters parameters)
        {
            // Contract.Requires(parameters != null);
            _parameters = parameters;
        }

        public NeuronParameters Parameters
        {
            get
            {
                //Contract.Ensures(Contract.Result<NeuronParameters>() != null);
                return _parameters;
            }
            set
            {
                // Contract.Requires(value != null);
                _parameters = value;
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_parameters != null);
        }
    }
}
