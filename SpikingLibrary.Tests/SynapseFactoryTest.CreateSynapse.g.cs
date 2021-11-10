// <copyright file="SynapseFactoryTest.CreateSynapse.g.cs" company="Monash University">Copyright � Monash University 2009</copyright>
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
using System.Collections.Generic;
using MathLib.Statistics;
using Microsoft.Pex.Framework.Explorable;
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
public void CreateSynapseThrowsContractException356()
{
    try
    {
      if (!PexContract.HasRequiredRuntimeContracts
               (typeof(SynapseFactory), (PexRuntimeContractsFlags)28799))
        Assert.Inconclusive
            ("assembly SpikingLibrary is not instrumented with runtime contracts");
      ConstantGenerator constantGenerator;
      SynapseFactory synapseFactory;
      Synapse synapse;
      constantGenerator = new ConstantGenerator(0);
      synapseFactory = PexInvariant.CreateInstance<SynapseFactory>();
      PexInvariant.SetField<INumberGenerator>((object)synapseFactory, 
                                              "_delayGenerator", (INumberGenerator)constantGenerator);
      PexInvariant.SetField<INumberGenerator>((object)synapseFactory, 
                                              "_weightGenerator", (INumberGenerator)constantGenerator);
      PexInvariant.CheckInvariant((object)synapseFactory);
      synapse = this.CreateSynapse(synapseFactory, (Neuron)null);
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
public void CreateSynapseThrowsContractException627()
{
    try
    {
      if (!PexContract.HasRequiredRuntimeContracts
               (typeof(SynapseFactory), (PexRuntimeContractsFlags)28799))
        Assert.Inconclusive
            ("assembly SpikingLibrary is not instrumented with runtime contracts");
      NormalRandomGenerator normalRandomGenerator;
      SynapseFactory synapseFactory;
      Synapse synapse;
      normalRandomGenerator = new NormalRandomGenerator(0, 0, 0);
      synapseFactory = PexInvariant.CreateInstance<SynapseFactory>();
      PexInvariant.SetField<INumberGenerator>((object)synapseFactory, 
                                              "_delayGenerator", (INumberGenerator)normalRandomGenerator);
      PexInvariant.SetField<INumberGenerator>((object)synapseFactory, 
                                              "_weightGenerator", (INumberGenerator)normalRandomGenerator);
      PexInvariant.CheckInvariant((object)synapseFactory);
      synapse = this.CreateSynapse(synapseFactory, (Neuron)null);
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
public void CreateSynapseThrowsContractException901()
{
    try
    {
      if (!PexContract.HasRequiredRuntimeContracts
               (typeof(SynapseFactory), (PexRuntimeContractsFlags)28799))
        Assert.Inconclusive
            ("assembly SpikingLibrary is not instrumented with runtime contracts");
      UniformRandomGenerator uniformRandomGenerator;
      SynapseFactory synapseFactory;
      Synapse synapse;
      uniformRandomGenerator = new UniformRandomGenerator(0, 0);
      synapseFactory = PexInvariant.CreateInstance<SynapseFactory>();
      PexInvariant.SetField<INumberGenerator>((object)synapseFactory, 
                                              "_delayGenerator", (INumberGenerator)uniformRandomGenerator);
      PexInvariant.SetField<INumberGenerator>((object)synapseFactory, 
                                              "_weightGenerator", (INumberGenerator)uniformRandomGenerator);
      PexInvariant.CheckInvariant((object)synapseFactory);
      synapse = this.CreateSynapse(synapseFactory, (Neuron)null);
      throw 
        new AssertFailedException("expected an exception of type ContractException");
    }
    catch(Exception ex)
    {
      if (!PexContract.IsContractException(ex))
        throw ex;
    }
}
    }
}