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

        [TestFixture]
        public class Constructor_Overload
        {
            [TestCase]
            public void Create_WorkingSet_From_Existing_WorkingSet()
            {
                var ws = new WorkingSet();
                var bogusTrackable = new BogusTrackable();
                ws.Add(bogusTrackable);

                var ws2 = new WorkingSet(ws);

                var result = ws2.FirstOrDefault();

                Assert.IsTrue(bogusTrackable == result);
            }
        }

        [TestFixture]
        public class Clear
        {
            [TestCase]
            public void Clear_WorkingSet()
            {
                var ws = new WorkingSet();
                var bogusTrackable = new BogusTrackable();
                ws.Add(bogusTrackable);

                ws.Clear();

                Assert.IsTrue(ws.Count == 0);
            }
        }

        [TestFixture]
        public class Contains
        {
            [TestCase]
            public void WorkingSet_Contains_Trackable()
            {
                var ws = new WorkingSet();
                var bogusTrackable = new BogusTrackable();
                ws.Add(bogusTrackable);

                Assert.Contains(bogusTrackable, ws.ToList());
            }

            [TestCase]
            public void WorkingSet_Does_Not_Contain_Trackable()
            {
                var ws = new WorkingSet();
                var bogusTrackable = new BogusTrackable();

                Assert.False(ws.Contains(bogusTrackable));
            }
        }

        [TestFixture]
        public class Remove
        {
            [TestCase]
            public void WorkingSet_Remove_Trackable()
            {
                var ws = new WorkingSet();
                var bogusTrackable = new BogusTrackable();
                ws.Add(bogusTrackable);
                ws.Remove(bogusTrackable);

                Assert.False(ws.Contains(bogusTrackable));
            }
        }

        [TestFixture]
        public class Count
        {
            [TestCase]
            public void WorkingSet_Count_0()
            {
                var ws = new WorkingSet();

                Assert.True(ws.Count == 0);
            }

            [TestCase]
            public void WorkingSet_Count_1()
            {
                var ws = new WorkingSet();
                var bogusTrackable = new BogusTrackable();
                ws.Add(bogusTrackable);

                Assert.True(ws.Count == 1);
            }
        }

        [TestFixture]
        public class CopyTo
        {
            [TestCase]
            public void CopyTo_Array()
            {
                var ws = new WorkingSet();
                var bogusTrackable = new BogusTrackable();
                ws.Add(bogusTrackable);

                var array = new ITrackable[1];

                ws.CopyTo(array, 0);

                Assert.IsTrue(array[0] == bogusTrackable);
            }
        }
    }
}
