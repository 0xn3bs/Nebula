using Castle.DynamicProxy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core.Tests
{
    public class ServiceInterceptorTests
    {
        [TestFixture]
        public class Intercept
        {
            [TestCase]
            public void TestInterception()
            {
                ServiceRegistry.Register<IBogusServiceInterface, BogusService>();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>();

                var expected = "BogusAltered!";

                var result = service.ReturnBogusString();

                Assert.True(result == expected);
            }
        }
    }
}
