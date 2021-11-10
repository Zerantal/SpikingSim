// <copyright file="SynapseProbeTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(SynapseProbe))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class SynapseProbeTest
    {
        [PexMethod]
        public SynapseProbe Constructor(Synapse target, int updateInterval)
        {
            SynapseProbe target01 = new SynapseProbe(target, updateInterval);
            return target01;
            // TODO: add assertions to method SynapseProbeTest.Constructor(Synapse, Int32)
        }
    }
}
