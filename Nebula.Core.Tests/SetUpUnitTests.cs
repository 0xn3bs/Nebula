using Nebula.Core.Tests.TestConstructs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core.Tests
{
    [SetUpFixture]
    public class SetUpUnitTests
    {
        [SetUp]
        public void SetUp()
        {
            TypeRegistry.Instance.Register(typeof(BogusTrackable));
            TypeRegistry.Instance.Register(typeof(BogusTrackableChild));

            var appService = new ApplicationService();

            var host = appService.Host();
        }
    }
}
