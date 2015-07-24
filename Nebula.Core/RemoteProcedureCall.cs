using Castle.DynamicProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    [DataContract]
    public class RemoteProcedureCall
    {
        [DataMember]
        private Guid _id;
        [DataMember]
        private ISession _session;

        [DataMember]
        private string _typeString;
        [DataMember]
        MethodInfo _method;
        [DataMember]
        object[] _arguments;
        [DataMember]
        Type[] _genericArguments;

        public RemoteProcedureCall(ISession session, IInvocation invocation)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));

            _id = Guid.NewGuid();
            _session = session;

            _typeString = invocation.Method.DeclaringType.AssemblyQualifiedName;
            _method = invocation.Method;
            _arguments = invocation.Arguments;
            _genericArguments = invocation.GenericArguments;
        }

        public void Execute()
        {
            var serialized = Serialize();
        }

        public string Serialize()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            var serializedJson = JsonConvert.SerializeObject(this, settings);
            return serializedJson;
        }
    }
}
