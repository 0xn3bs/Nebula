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
        private object[] _parameters;
        [DataMember]
        private Type[] _genericArguments;
        [DataMember]
        private Type _returnType;

        [DataMember]
        private bool _isGenericMethod;

        public RemoteProcedureCall()
        {

        }

        public RemoteProcedureCall(ISession session, IInvocation invocation)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));

            _id = Guid.NewGuid();
            _session = session;

            _typeFullName = invocation.InvocationTarget.GetType().FullName;
            _methodName = invocation.Method.Name;
            _parameters = invocation.Arguments;
            _genericArguments = invocation.GenericArguments;
            _returnType = invocation.Method.ReturnType;
            _isGenericMethod = invocation.Method.IsGenericMethod;
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
                settings.Binder = TypeRegistry.Instance;
                settings.TypeNameHandling = TypeNameHandling.All;
                settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                var deserialized = JsonConvert.DeserializeObject(responseString, settings);

                return deserialized;
            }
        }

        private MethodInfo GetMethodWithMatchingParameters(List<MethodInfo> methods)
        {
            MethodInfo candidateMethod = methods.FirstOrDefault();

            foreach (var m in methods)
            {
                var innerCandidateMethod = candidateMethod;

                var pars = m.GetParameters();

                var argc = _parameters.Count();

                for (int i = 0; i < argc; ++i)
                {
                    if (pars[0].GetType() != _parameters[i].GetType())
                    {
                        innerCandidateMethod = m;
                    }
                    else
                    {
                        innerCandidateMethod = candidateMethod;
                        break;
                    }
                }

                candidateMethod = innerCandidateMethod;
            }

            if (candidateMethod == null)
            {
                throw new Exception($"Unable to find method {candidateMethod.Name} with matching parameters.");
            }

            return candidateMethod;
        }

        public object Result()
        {
            var type = TypeRegistry.Instance.GetRegisteredTypeByName(_typeFullName);

            var instance = Activator.CreateInstance(type);

            if (_isGenericMethod)
            {
                var methods = type.GetMethods().Where(x => x.Name == _methodName && x.IsGenericMethod &&
                                                        x.GetParameters().Count() == _parameters.Count() &&
                                                        x.GetGenericArguments().Count() == _genericArguments.Count()).ToList();

                var candidateMethod = GetMethodWithMatchingParameters(methods);

                var method = candidateMethod.MakeGenericMethod(_genericArguments);
                var result = method.Invoke(instance, _parameters);
                return result;
            }
            else
            {
                var methods = type.GetMethods().Where(x => x.Name == _methodName && 
                                                        !x.IsGenericMethod && 
                                                        x.ReturnType == _returnType && 
                                                        x.GetParameters().Count() == _parameters.Count()).ToList();

                var candidateMethod = GetMethodWithMatchingParameters(methods);

                var result = candidateMethod.Invoke(instance, _parameters);
                return result;
            }
        }

        public string Serialize()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            var serializedJson = JsonConvert.SerializeObject(this, settings);
            return serializedJson;
        }

        public static RemoteProcedureCall Deserialize(string rpcJson)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Binder = TypeRegistry.Instance;
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            var deserialized = JsonConvert.DeserializeObject<RemoteProcedureCall>(rpcJson, settings);
            return deserialized;
        }
    }
}
