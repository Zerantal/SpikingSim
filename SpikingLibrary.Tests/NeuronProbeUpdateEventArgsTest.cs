// <copyright file="NeuronProbeUpdateEventArgsTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(NeuronProbeUpdateEventArgs))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class NeuronProbeUpdateEventArgsTest
    {
    }
}
