#region

using System;
using System.Diagnostics.Contracts;

#endregion

namespace SpikingLibrary
{
    [ContractClassFor(typeof (ISynapseFactory))]
// ReSharper disable InconsistentNaming
    internal abstract class ISynapseFactoryContract : ISynapseFactory
// ReSharper restore InconsistentNaming
    {
        #region ISynapseFactory Members

        public Synapse CreateSynapse()
        {            
            //Contract.Ensures(Contract.Result<Synapse>() != null);

            throw new NotImplementedException();
        }

        #endregion

        #region IDeepCloneable<ISynapseFactory> Members

        public ISynapseFactory DeepClone()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}