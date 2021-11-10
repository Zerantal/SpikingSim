// <copyright file="ScheduledEventItemTest.cs" company="Monash University">Copyright © Monash University 2009</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpikingLibrary;

namespace SpikingLibrary
{
    [TestClass]
    [PexClass(typeof(ScheduledEventItem))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class ScheduledEventItemTest
    {
        [PexMethod]
        public void TimeIntervalSet(ScheduledEventItem target, int value)
        {
            target.TimeInterval = value;
            // TODO: add assertions to method ScheduledEventItemTest.TimeIntervalSet(ScheduledEventItem, Int32)
        }
        [PexMethod]
        public int TimeIntervalGet(ScheduledEventItem target)
        {
            int result = target.TimeInterval;
            return result;
            // TODO: add assertions to method ScheduledEventItemTest.TimeIntervalGet(ScheduledEventItem)
        }
        [PexMethod]
        public void ScheduledEventSet(ScheduledEventItem target, ScheduledEvent value)
        {
            target.ScheduledEvent = value;
            // TODO: add assertions to method ScheduledEventItemTest.ScheduledEventSet(ScheduledEventItem, ScheduledEvent)
        }
        [PexMethod]
        public ScheduledEvent ScheduledEventGet(ScheduledEventItem target)
        {
            ScheduledEvent result = target.ScheduledEvent;
            return result;
            // TODO: add assertions to method ScheduledEventItemTest.ScheduledEventGet(ScheduledEventItem)
        }
        [PexMethod]
        public ScheduledEventItem Constructor(ScheduledEvent callback, int time)
        {
            ScheduledEventItem target = new ScheduledEventItem(callback, time);
            return target;
            // TODO: add assertions to method ScheduledEventItemTest.Constructor(ScheduledEvent, Int32)
        }
    }
}
