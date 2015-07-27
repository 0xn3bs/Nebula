using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    public class TypeRegistry : SerializationBinder
    {
        private static TypeRegistry _instance;

        private HashSet<Type> registered_types;

        private TypeRegistry()
        {
            registered_types = new HashSet<Type>();
        }

        public static TypeRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TypeRegistry();
                }

                return _instance;
            }
        }

        public void Register(Type type)
        {
            registered_types.Add(type);
        }

        public Type GetRegisteredTypeByName(string typeName)
        {
            return registered_types.FirstOrDefault(x => x.FullName == typeName || x.Name == typeName);
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            return Type.GetType(typeName) ?? GetRegisteredTypeByName(typeName);
        }
    }
}
