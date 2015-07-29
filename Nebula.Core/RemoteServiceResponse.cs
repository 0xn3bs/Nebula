using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    [DataContract]
    public class RemoteServiceResponse
    {
        [DataMember]
        private object _result;
        [DataMember]
        private Exception _exception;

        public RemoteServiceResponse()
        {
            _result = null;
            _exception = null;
        }

        public RemoteServiceResponse(object result, Exception ex)
        {
            _result = result;
            _exception = ex;
        }

        public Exception Exception
        {
            get { return _exception; }
        }

        public object Result
        {
            get { return _result; }
        }
    }
}
