// <copyright file="NeuronCollectionTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;
using System.Runtime.Serialization;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(NeuronCollection))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class NeuronCollectionTest
    {
        [PexMethod]
        public void NameSet([PexAssumeUnderTest]NeuronCollection target, string value)
        {
            target.Name = value;
            // TODO: add assertions to method NeuronCollectionTest.NameSet(NeuronCollection, String)
        }
        [PexMethod]
        public string NameGet([PexAssumeUnderTest]NeuronCollection target)
        {
            string result = target.Name;
            return result;
            // TODO: add assertions to method NeuronCollectionTest.NameGet(NeuronCollection)
        }
        [PexMethod]
        public NeuronCollection Constructor01(string name)
        {
            NeuronCollection target = new NeuronCollection(name);
            return target;
            // TODO: add assertions to method NeuronCollectionTest.Constructor01(String)
        }
        [PexMethod]
        public NeuronCollection Constructor()
        {
            NeuronCollection target = new NeuronCollection();
            return target;
            // TODO: add assertions to method NeuronCollectionTest.Constructor()
        }
        [PexMethod]
        public void ConnectTo(
            [PexAssumeUnderTest]NeuronCollection target,
            NeuronCollection neuronGroup,
            ISynapseFactory synapseFactory
        )
        {
            target.ConnectTo(neuronGroup, synapseFactory);
            // TODO: add assertions to method NeuronCollectionTest.ConnectTo(NeuronCollection, NeuronCollection, ISynapseFactory)
        }
    }
}
