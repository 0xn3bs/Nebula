using Nebula.Core.Tests.TestConstructs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core.Tests
{
    public class WorkingSetTests
    {
        [TestFixture]
        public class Add
        {
            [TestCase]
            public void AddValidTrackableObject()
            {
                var ws = new WorkingSet();
                var bogusTrackable = new BogusTrackable();
                bogusTrackable.Test = "TEST!";

                ws.Add(bogusTrackable);

                var result = ws.FirstOrDefault(x => x.GetType() == typeof(BogusTrackable));

                Assert.IsTrue(bogusTrackable.Equals(result));
            }
        }
    }
}
