// <copyright file="SynapseTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;
using ZedGraph;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(Synapse))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class SynapseTest
    {
        [PexMethod]
        public Neuron PostsynapticNeuronGet([PexAssumeUnderTest]Synapse target)
        {
            Neuron result = target.PostsynapticNeuron;
            return result;
            // TODO: add assertions to method SynapseTest.PostsynapticNeuronGet(Synapse)
        }
        [PexMethod]
        public int AxonalDelayGet([PexAssumeUnderTest]Synapse target)
        {
            int result = target.AxonalDelay;
            return result;
            // TODO: add assertions to method SynapseTest.AxonalDelayGet(Synapse)
        }
        [PexMethod]
        public Synapse Constructor01(
            Neuron postsynapticNeuron,
            int delay,
            double efficacy
        )
        {
            Synapse target = new Synapse(postsynapticNeuron, delay, efficacy);
            return target;
            // TODO: add assertions to method SynapseTest.Constructor01(Neuron, Int32, Double)
        }
    }
}
