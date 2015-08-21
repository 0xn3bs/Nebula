using Castle.DynamicProxy;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Nebula.Core
{
    public class ServiceProxyGenerator
    {
        public static IServiceInterface GetService<IServiceInterface>(ISession session = null)
            where IServiceInterface : class
        {
            ProxyGenerator _generator = new ProxyGenerator(new PersistentProxyBuilder());
            ServiceCallInterceptor _serviceInterceptor = new ServiceCallInterceptor(session);

            ProxyGenerationOptions options = new ProxyGenerationOptions();

            var type = typeof(IServiceInterface);

            var remoteServiceAttribute = type.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(RemoteServiceAttribute));

            var remoteServiceAttributeCtr = remoteServiceAttribute.Constructor;
            var remoteServiceAttributeCtrArgs = remoteServiceAttribute.ConstructorArguments;

            List<object> attribArgs = new List<object>();

            foreach (var arg in remoteServiceAttributeCtrArgs)
            {
                attribArgs.Add(arg.Value);
            }

            var attributeBuilder = new CustomAttributeBuilder(remoteServiceAttributeCtr, attribArgs.ToArray());

            options.AdditionalAttributes.Add(attributeBuilder);

            var proxy = _generator.CreateInterfaceProxyWithoutTarget<IServiceInterface>(options, _serviceInterceptor);

            return proxy as IServiceInterface;
        }
    }
}
