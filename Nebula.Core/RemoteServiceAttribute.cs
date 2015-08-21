using System;

namespace Nebula.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class RemoteServiceAttribute : System.Attribute
    {
        public RemoteServiceAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
