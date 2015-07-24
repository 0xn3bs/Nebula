using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core.Tests.TestConstructs
{
    [DataContract]
    public class BogusTrackable : TrackableObject
    {
        public string _test;

        [DataMember]
        public string Test
        {
            get { return _test; }
            set { _test = value; }
        }
    }
}
