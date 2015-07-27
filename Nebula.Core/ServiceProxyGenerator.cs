using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    public class ServiceProxyGenerator
    {
        public static IServiceInterface GetService<IServiceInterface>(ISession session = null)
            where IServiceInterface : class
        {
            var service = ServiceFactory.Instance.GetInstance<IServiceInterface>();

            ProxyGenerator _generator = new ProxyGenerator(new PersistentProxyBuilder());
            ServiceCallInterceptor _serviceInterceptor = new ServiceCallInterceptor(session);
            var proxy = _generator.CreateInterfaceProxyWithTarget(service, _serviceInterceptor);

            return proxy;
        }
    }
}
