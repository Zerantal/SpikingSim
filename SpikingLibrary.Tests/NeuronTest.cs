// <copyright file="NeuronTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(Neuron))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class NeuronTest
    {
        [PexMethod]
        public void NeuronTypeSet([PexAssumeUnderTest]Neuron target, NeuronClass value)
        {
            target.NeuronType = value;
            // TODO: add assertions to method NeuronTest.NeuronTypeSet(Neuron, NeuronClass)
        }
        [PexMethod]
        public NeuronClass NeuronTypeGet([PexAssumeUnderTest]Neuron target)
        {
            NeuronClass result = target.NeuronType;
            return result;
            // TODO: add assertions to method NeuronTest.NeuronTypeGet(Neuron)
        }
        [PexMethod]
        public int IdGet([PexAssumeUnderTest]Neuron target)
        {
            int result = target.Id;
            return result;
            // TODO: add assertions to method NeuronTest.IdGet(Neuron)
        }
        [PexMethod]
        public Neuron Constructor(NeuronClass type)
        {
            Neuron target = new Neuron(type);
            return target;
            // TODO: add assertions to method NeuronTest.Constructor(NeuronClass)
        }
    }
}
