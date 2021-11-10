// <copyright file="PeriodicInputSignalTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;
using MathLib.Statistics;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(PeriodicInputSignal))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class PeriodicInputSignalTest
    {
        [PexMethod]
        public void Stop([PexAssumeUnderTest]PeriodicInputSignal target)
        {
            target.Stop();
            // TODO: add assertions to method PeriodicInputSignalTest.Stop(PeriodicInputSignal)
        }
        [PexMethod]
        public void Start([PexAssumeUnderTest]PeriodicInputSignal target)
        {
            target.Start();
            // TODO: add assertions to method PeriodicInputSignalTest.Start(PeriodicInputSignal)
        }
    }
}
