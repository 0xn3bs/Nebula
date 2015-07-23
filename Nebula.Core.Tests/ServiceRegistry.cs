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
    public interface IBogusServiceInterface
    {
        string ReturnBogusString();
    }

    public class BogusService : IBogusServiceInterface
    {
        public string ReturnBogusString()
        {
            return "Bogus!";
        }
    }

    public class ServiceRegistryTests
    {
        [TestFixture]
        public class Register
        {
            [TestCase]
            public void RegisterValidService()
            {
                ServiceRegistry.Register<IBogusServiceInterface, BogusService>();

                Assert.Pass();
            }
        }

        [TestFixture]
        public class GetInstance
        {
            [TestCase]
            public void GetValidService()
            {
                ServiceRegistry.Register<IBogusServiceInterface, BogusService>();

                var service = ServiceRegistry.GetInstance<IBogusServiceInterface>();

                Assert.IsNotNull(service);
            }

            [TestCase]
            public void CallValidServiceMethod()
            {
                ServiceRegistry.Register<IBogusServiceInterface, BogusService>();

                var service = ServiceRegistry.GetInstance<IBogusServiceInterface>();

                var expectedString = "Bogus!";

                var result = service.ReturnBogusString();

                Assert.True(result == expectedString);
            }
        }
    }
}
