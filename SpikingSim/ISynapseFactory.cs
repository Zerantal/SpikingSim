using System.Diagnostics.Contracts;

using Util;

namespace SpikingLibrary
{
    [ContractClass(typeof(ISynapseFactoryContract))]
    public interface ISynapseFactory : IDeepCloneable<ISynapseFactory>
    {
        Synapse CreateSynapse();
    }
}
