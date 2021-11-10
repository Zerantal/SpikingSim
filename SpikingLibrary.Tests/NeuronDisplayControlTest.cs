// <copyright file="NeuronDisplayControlTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(NeuronDisplayControl))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class NeuronDisplayControlTest
    {
        [PexMethod]
        public NeuronDisplayControl Constructor()
        {
            NeuronDisplayControl target = new NeuronDisplayControl();
            return target;
            // TODO: add assertions to method NeuronDisplayControlTest.Constructor()
        }
    }
}
