using Castle.DynamicProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private string _typeFullName;
        [DataMember]
        private string _methodName;

        [DataMember]
        private object[] _arguments;
        [DataMember]
        private Type[] _genericArguments;

        public RemoteProcedureCall()
        {

        }

        public RemoteProcedureCall(ISession session, IInvocation invocation)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));

            _id = Guid.NewGuid();
            _session = session;

            _typeFullName = invocation.InvocationTarget.GetType().AssemblyQualifiedName;
            _methodName = invocation.Method.Name;
            _arguments = invocation.Arguments;
            _genericArguments = invocation.GenericArguments;
        }

        public async Task<object> Execute()
        {
            var serialized = Serialize();

            var appServicePrefix = ConfigurationManager.AppSettings.GetValues("ApplicationServicePrefix").FirstOrDefault();

            using (var client = new HttpClient())
            {
                var content = new StringContent(serialized);

                var response = await client.PostAsync(appServicePrefix, content);

                var responseBytes = await response.Content.ReadAsByteArrayAsync();

                var responseString = System.Text.Encoding.UTF8.GetString(responseBytes);

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.TypeNameHandling = TypeNameHandling.All;
                settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
                var deserialized = JsonConvert.DeserializeObject(responseString, settings);

                return deserialized;
            }
        }

        public object Result()
        {
            var type = Type.GetType(_typeFullName);

            var instance = Activator.CreateInstance(type);

            var result = type.InvokeMember(_methodName, BindingFlags.InvokeMethod, null, instance, _arguments);

            return result;
        }

        public string Serialize()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            var serializedJson = JsonConvert.SerializeObject(this, settings);
            return serializedJson;
        }

        public static RemoteProcedureCall Deserialize(string rpcJson)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            var deserialized = JsonConvert.DeserializeObject<RemoteProcedureCall>(rpcJson, settings);
            return deserialized;
        }
    }
}
