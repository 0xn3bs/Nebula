using Castle.DynamicProxy;
using Nebula.Core.Tests.TestConstructs;
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
            public void TestBasicRPC()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                TypeRegistry.Instance.Register(typeof(BogusTrackable));

                var session = new Session();

                var bogusTrackable = new BogusTrackable();
                bogusTrackable.Test = "Test!";

                session.WorkingSet.Add(bogusTrackable);

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                var expected = "Bogus!";

                var result = service.ReturnBogusString();   //  RPC

                Assert.True(result == expected);
            }

            [TestCase]
            public void TestGenericRPC()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                long expected = 1234;

                var result = service.ReturnBogusGenericObject<long>(1234);   //  RPC

                Assert.True(result == expected);
            }
        }
    }
}
