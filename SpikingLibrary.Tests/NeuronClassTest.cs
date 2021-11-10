// <copyright file="NeuronClassTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(NeuronClass))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class NeuronClassTest
    {
        [PexMethod]
        public string LabelGet([PexAssumeUnderTest]NeuronClass target)
        {
            string result = target.Label;
            return result;
            // TODO: add assertions to method NeuronClassTest.LabelGet(NeuronClass)
        }
        [PexMethod]
        public NeuronClass Constructor02(
            double a,
            double b,
            double c,
            double d,
            double I,
            string className
        )
        {
            NeuronClass target = new NeuronClass(a, b, c, d, I, className);
            return target;
            // TODO: add assertions to method NeuronClassTest.Constructor02(Double, Double, Double, Double, Double, String)
        }
        [PexMethod]
        public NeuronClass Constructor01(
            double a,
            double b,
            double c,
            double d,
            double I
        )
        {
            NeuronClass target = new NeuronClass(a, b, c, d, I);
            return target;
            // TODO: add assertions to method NeuronClassTest.Constructor01(Double, Double, Double, Double, Double)
        }
        [PexMethod]
        public NeuronClass Constructor()
        {
            NeuronClass target = new NeuronClass();
            return target;
            // TODO: add assertions to method NeuronClassTest.Constructor()
        }
    }
}
