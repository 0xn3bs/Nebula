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
    public class ServiceRegistryTests
    {
        [TestFixture]
        public class Register
        {
            [TestCase]
            public void RegisterValidService()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                Assert.Pass();
            }
        }

        [TestFixture]
        public class GetInstance
        {
            [TestCase]
            public void GetValidService()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var service = ServiceFactory.Instance.GetInstance<IBogusServiceInterface>();

                Assert.IsNotNull(service);
            }
        }
    }
}
