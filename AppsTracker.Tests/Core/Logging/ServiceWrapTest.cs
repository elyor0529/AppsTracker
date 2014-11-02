﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppsTracker.Logging;
using AppsTracker.Tests.Fakes.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppsTracker.Tests.Core.Logging
{
    [TestClass]
    public class ServiceWrapTest
    {
        bool _testEventFired = false;
        bool _disposeEventFired = false;

        [TestMethod]
        public void TestService()
        {
            ServiceWrap<DisposableMock> service = new ServiceWrap<DisposableMock>(() => new DisposableMock(),
                d => { d.TestEvent += d_TestEvent; d.DisposeTestEvent += d_DisposeTestEvent; },
                d => { d.DisposeTestEvent -= d_TestEvent; });

            service.Enabled = true;

            service.CallOnService(s => s.FireEvent());

            Assert.IsTrue(_testEventFired, "On Init service method failed");

            service.Enabled = false;

            Assert.IsTrue(_disposeEventFired, "On Dispose service method failed");

            service.CallOnService(s => s.FireEvent());

            Assert.IsTrue(_testEventFired, "Call on action should have failed");

            var mock = service.Component;

            service.CallOnService(s => s.FireEvent());

            Assert.IsFalse(_testEventFired, "Init on property failed");
        }


        void d_TestEvent(object sender, EventArgs e)
        {
            _testEventFired = !_testEventFired;
        }

        void d_DisposeTestEvent(object sender, EventArgs e)
        {
            _disposeEventFired = true;
        }
    }
}