using Nebula.Core.Tests.TestConstructs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Nebula.Core.Tests
{
    public class RemoteServiceCallTests
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

            [TestCase]
            public async Task Async_Service_Method_Task()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                await service.DoSomethingAsync();

                Assert.Pass();
            }

            [TestCase]
            [ExpectedException(ExpectedMessage = "Some Service Exception")]
            public void Service_Method_That_Throws_Exception()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                service.ThrowAnException("Some Service Exception");
            }

            [TestCase]
            public void Service_Method_Utilizing_Interfaces()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                IBogusTrackable expected = new BogusTrackable();
                expected.Test = "Some string";

                var result = service.ReturnBogusObject(expected);

                Assert.True(result.Test == expected.Test);
            }

            [TestCase, MaxTime(5000)]
            [Ignore("For obvious reasons, I don't want this test running every time.")]
            public void Service_Method_Slow()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                var expected = "12345";

                string result = service.SlowServiceCall(expected);

                Assert.True(result == expected);
            }

            [TestCase, MaxTime(5000)]
            [Ignore("For obvious reasons, I don't want this test running every time.")]
            public async Task Service_Method_Slow_Async()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                var expected = "12345";

                string result = await service.SlowServiceCallAsync(expected);

                Assert.True(result == expected);
            }

            [TestCase]
            public void Service_Method_That_Returns_Nullable_Type_That_Is_Null()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                var result = service.ReturnNullableType(true);

                Assert.True(result == null);
            }

            [TestCase]
            public void Service_Method_That_Returns_Nullable_Type_That_Has_Value()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                var result = service.ReturnNullableType(false);

                Assert.True(result != null);
            }

            [TestCase]
            [Ignore("To be addressed later")]
            public void Service_Method_That_Returns_Guid()
            {
                ServiceFactory.Instance.Register<IBogusServiceInterface, BogusService>();

                var session = new Session();

                var service = ServiceProxyGenerator.GetService<IBogusServiceInterface>(session);

                var result = service.ReturnGuid();

                Assert.True(result != null);
            }
        }
    }
}
