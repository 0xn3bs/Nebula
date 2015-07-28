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
    public class RemoteProcedureCallTests
    {
        [TestFixture]
        public class Execute
        {
            [TestCase]
            public void Service_Method_With_No_Arguments()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                TypeRegistry.Instance.Register(typeof(BogusTrackable));

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                var expected = "Bogus!";

                var result = service.ReturnBogusString();

                Assert.True(result == expected);
            }

            [TestCase]
            public void Service_Method_Overload_With_One_Argument()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                TypeRegistry.Instance.Register(typeof(BogusTrackable));

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                var expected = "Bogus String!";

                var result = service.ReturnBogusString(expected);

                Assert.True(result == expected);
            }

            [TestCase]
            public void Service_Method_Overload_With_Two_Arguments()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                TypeRegistry.Instance.Register(typeof(BogusTrackable));

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                var expected = "Bogus String2!";

                var result = service.ReturnBogusString(expected, 234);

                Assert.True(result == expected);
            }

            [TestCase]
            public void Generic_Service_Method_With_Generic_Type_Argument()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                long expected = 1234;

                var result = service.ReturnBogusGenericObject<long>(1234);

                Assert.True(result == expected);
            }

            [TestCase]
            public void Generic_Service_Method_With_No_Arguments()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                long expected = 0;

                var result = service.ReturnBogusGenericObject<long>();

                Assert.True(result == expected);
            }

            [TestCase]
            public void Generic_Service_Method_Overload_With_Two_Generic_Type_Arguments()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                long expected = 4567;

                var result = service.ReturnBogusGenericObject<long, string>(4567, "asdasd");

                Assert.True(result == expected);
            }

            [TestCase]
            public void Service_Method_Call_Utilizing_Child_Class()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                bool expected = true;

                var child = new BogusTrackableChild();

                var result = service.ReturnBogusBool(child);

                Assert.True(result == expected);
            }

            [TestCase]
            public async Task Async_Service_Method_Call_Utilizing_Child_Class()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                var expected = "Bogus String3!";

                var result = await service.ReturnBogusStringAsync(expected);

                Assert.True(result == expected);
            }
        }
    }
}
