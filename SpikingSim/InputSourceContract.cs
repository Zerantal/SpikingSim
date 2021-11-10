using System;
using System.Diagnostics.Contracts;

namespace SpikingLibrary
{
    [ContractClassFor(typeof(InputSource))]
    internal abstract class InputSourceContract : InputSource
    {
        #region IInputSource Members

        public override void Start(int startTime)
        {
            // Contract.Requires(startTime > 0);
            throw new NotImplementedException();
        }

        #endregion
    }
}
