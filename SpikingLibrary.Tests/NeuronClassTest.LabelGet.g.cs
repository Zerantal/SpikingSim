// <copyright file="NeuronClassTest.LabelGet.g.cs" company="Monash University">Copyright � Monash University 2009</copyright>
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;

namespace SpikingLibrary
{
    public partial class NeuronClassTest
    {
[TestMethod]
[PexGeneratedBy(typeof(NeuronClassTest))]
public void LabelGet789()
{
    NeuronClass neuronClass;
    string s;
    neuronClass = new NeuronClass(0, 0, 0, 0, 0, (string)null);
    s = this.LabelGet(neuronClass);
    Assert.AreEqual<string>((string)null, s);
    Assert.IsNotNull((object)neuronClass);
    Assert.AreEqual<string>((string)null, neuronClass.Label);
}
    }
}
