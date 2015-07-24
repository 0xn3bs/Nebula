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
        static ApplicationService appService;

        [SetUp]
        public void SetUp()
        {
            ApplicationService appService = new ApplicationService();
            var host = appService.Host();
        }
    }
}
