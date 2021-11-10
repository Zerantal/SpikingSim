// <copyright file="SynapseFactoryTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;
using MathLib.Statistics;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(SynapseFactory))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class SynapseFactoryTest
    {
        [PexMethod]
        public Synapse CreateSynapse([PexAssumeUnderTest]SynapseFactory target, Neuron postsynapticNeuron)
        {
            Synapse result = target.CreateSynapse(postsynapticNeuron);
            return result;
            // TODO: add assertions to method SynapseFactoryTest.CreateSynapse(SynapseFactory, Neuron)
        }
        [PexMethod]
        public SynapseFactory Constructor(INumberGenerator delayGenerators, INumberGenerator efficacyGenerator)
        {
            SynapseFactory target = new SynapseFactory(delayGenerators, efficacyGenerator);
            return target;
            // TODO: add assertions to method SynapseFactoryTest.Constructor(INumberGenerator, INumberGenerator)
        }
    }
}
