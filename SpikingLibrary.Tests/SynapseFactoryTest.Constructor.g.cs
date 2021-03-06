// <copyright file="SynapseFactoryTest.Constructor.g.cs" company="Monash University">Copyright ? Monash University 2009</copyright>
// <auto-generated>
// This file contains automatically generated unit tests.
// Do NOT modify this file manually.
// 
// When Pex is invoked again,
// it might remove or update any previously generated unit tests.
// 
// If the contents of this file becomes outdated, e.g. if it does not
// compile anymore, you may delete this file and invoke Pex again.
// </auto-generated>
using System;
using MathLib.Statistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;
using Microsoft.Pex.Engine.Exceptions;

namespace SpikingLibrary
{
    public partial class SynapseFactoryTest
    {
[TestMethod]
[PexGeneratedBy(typeof(SynapseFactoryTest))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException378()
{
    try
    {
      if (!PexContract.HasRequiredRuntimeContracts
               (typeof(SynapseFactory), (PexRuntimeContractsFlags)28799))
        Assert.Inconclusive
            ("assembly SpikingLibrary is not instrumented with runtime contracts");
      SynapseFactory synapseFactory;
      synapseFactory =
        this.Constructor((INumberGenerator)null, (INumberGenerator)null);
      throw 
        new AssertFailedException("expected an exception of type ContractException");
    }
    catch(Exception ex)
    {
      if (!PexContract.IsContractException(ex))
        throw ex;
    }
}
[TestMethod]
[PexGeneratedBy(typeof(SynapseFactoryTest))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException165()
{
    try
    {
      if (!PexContract.HasRequiredRuntimeContracts
               (typeof(SynapseFactory), (PexRuntimeContractsFlags)28799))
        Assert.Inconclusive
            ("assembly SpikingLibrary is not instrumented with runtime contracts");
      ConstantGenerator constantGenerator;
      SynapseFactory synapseFactory;
      constantGenerator = new ConstantGenerator(0);
      synapseFactory = this.Constructor
                           ((INumberGenerator)constantGenerator, (INumberGenerator)null);
      throw 
        new AssertFailedException("expected an exception of type ContractException");
    }
    catch(Exception ex)
    {
      if (!PexContract.IsContractException(ex))
        throw ex;
    }
}
[TestMethod]
[PexGeneratedBy(typeof(SynapseFactoryTest))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException758()
{
    try
    {
      if (!PexContract.HasRequiredRuntimeContracts
               (typeof(SynapseFactory), (PexRuntimeContractsFlags)28799))
        Assert.Inconclusive
            ("assembly SpikingLibrary is not instrumented with runtime contracts");
      NormalRandomGenerator normalRandomGenerator;
      SynapseFactory synapseFactory;
      normalRandomGenerator = new NormalRandomGenerator(0, 0, 0);
      synapseFactory = this.Constructor
                           ((INumberGenerator)normalRandomGenerator, (INumberGenerator)null);
      throw 
        new AssertFailedException("expected an exception of type ContractException");
    }
    catch(Exception ex)
    {
      if (!PexContract.IsContractException(ex))
        throw ex;
    }
}
[TestMethod]
[PexGeneratedBy(typeof(SynapseFactoryTest))]
[PexRaisedContractException(PexExceptionState.Expected)]
public void ConstructorThrowsContractException778()
{
    try
    {
      if (!PexContract.HasRequiredRuntimeContracts
               (typeof(SynapseFactory), (PexRuntimeContractsFlags)28799))
        Assert.Inconclusive
            ("assembly SpikingLibrary is not instrumented with runtime contracts");
      UniformRandomGenerator uniformRandomGenerator;
      SynapseFactory synapseFactory;
      uniformRandomGenerator = new UniformRandomGenerator(0, 0);
      synapseFactory = this.Constructor
                           ((INumberGenerator)uniformRandomGenerator, (INumberGenerator)null);
      throw 
        new AssertFailedException("expected an exception of type ContractException");
    }
    catch(Exception ex)
    {
      if (!PexContract.IsContractException(ex))
        throw ex;
    }
}
[TestMethod]
[PexGeneratedBy(typeof(SynapseFactoryTest))]
public void Constructor139()
{
    NormalRandomGenerator normalRandomGenerator;
    SynapseFactory synapseFactory;
    normalRandomGenerator = new NormalRandomGenerator(0, 0, 0);
    synapseFactory = this.Constructor((INumberGenerator)normalRandomGenerator, 
                                      (INumberGenerator)normalRandomGenerator);
    Assert.IsNotNull((object)synapseFactory);
}
    }
}
