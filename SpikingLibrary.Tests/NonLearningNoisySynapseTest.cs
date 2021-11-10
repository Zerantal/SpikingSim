// <copyright file="NonLearningNoisySynapseTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;
using MathLib.Statistics;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(NonLearningNoisySynapse))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class NonLearningNoisySynapseTest
    {
        [PexMethod]
        public double EfficacyGet([PexAssumeUnderTest]NonLearningNoisySynapse target)
        {
            double result = target.Weight;
            return result;
            // TODO: add assertions to method NonLearningNoisySynapseTest.EfficacyGet(NonLearningNoisySynapse)
        }
    }
}
