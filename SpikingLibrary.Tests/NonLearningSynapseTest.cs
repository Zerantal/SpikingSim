// <copyright file="NonLearningSynapseTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(NonLearningSynapse))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class NonLearningSynapseTest
    {
        [PexMethod]
        public NonLearningSynapse Constructor(
            Neuron postsynapticNeuron,
            int delay,
            int efficacy
        )
        {
            NonLearningSynapse target = new NonLearningSynapse(postsynapticNeuron, delay, efficacy);
            return target;
            // TODO: add assertions to method NonLearningSynapseTest.Constructor(Neuron, Int32, Int32)
        }
    }
}
