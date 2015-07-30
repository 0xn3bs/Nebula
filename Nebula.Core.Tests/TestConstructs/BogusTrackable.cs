using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core.Tests.TestConstructs
{
    public interface IBogusTrackable
    {
        string Test { get; set; }
    }

    [DataContract]
    public class BogusTrackable : TrackableObject, IBogusTrackable
    {
        public string _test;

        [DataMember]
        public string Test
        {
            get { return _test; }
            set { _test = value; }
        }
    }

    public class BogusTrackableChild : BogusTrackable
    {
        public string _test2;

        [DataMember]
        public string Test2
        {
            get { return _test; }
            set { _test = value; }
        }
    }
}
