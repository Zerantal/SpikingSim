// <copyright file="NonLearningSynapseFactoryTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;
using MathLib.Statistics;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(NonLearningSynapseFactory))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class NonLearningSynapseFactoryTest
    {
        [PexMethod]
        public Synapse CreateSynapse([PexAssumeUnderTest]NonLearningSynapseFactory target, Neuron postsynapticNeuron)
        {
            Synapse result = target.CreateSynapse(postsynapticNeuron);
            return result;
            // TODO: add assertions to method NonLearningSynapseFactoryTest.CreateSynapse(NonLearningSynapseFactory, Neuron)
        }
        [PexMethod]
        public NonLearningSynapseFactory Constructor(INumberGenerator delayGenerators, INumberGenerator efficacyGenerator)
        {
            NonLearningSynapseFactory target = new NonLearningSynapseFactory(delayGenerators, efficacyGenerator)
              ;
            return target;
            // TODO: add assertions to method NonLearningSynapseFactoryTest.Constructor(INumberGenerator, INumberGenerator)
        }
    }
}
