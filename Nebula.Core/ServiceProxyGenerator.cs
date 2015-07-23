﻿using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    public class ServiceProxyGenerator
    {
        public static IServiceInterface GetService<IServiceInterface>()
            where IServiceInterface : class
        {
            var service = ServiceRegistry.GetInstance<IServiceInterface>();

            ProxyGenerator _generator = new ProxyGenerator(new PersistentProxyBuilder());
            ServiceInterceptor _myinterceptor = new ServiceInterceptor();
            var proxy = _generator.CreateInterfaceProxyWithTarget(service, _myinterceptor);

            return proxy;
        }
    }
}
