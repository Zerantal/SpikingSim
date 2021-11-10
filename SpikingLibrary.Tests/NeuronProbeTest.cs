// <copyright file="NeuronProbeTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(NeuronProbe))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class NeuronProbeTest
    {
        [PexMethod]
        public NeuronProbe Constructor(Neuron target, int updateInterval)
        {
            NeuronProbe target01 = new NeuronProbe(target, updateInterval);
            return target01;
            // TODO: add assertions to method NeuronProbeTest.Constructor(Neuron, Int32)
        }
    }
}
