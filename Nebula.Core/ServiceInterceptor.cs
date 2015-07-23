using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    public class ServiceInterceptor : IInterceptor
    {
        bool _serviceLocal = true;

        public void Intercept(IInvocation invocation)
        {
            if (_serviceLocal)
            {
                invocation.Proceed();
            }
            else
            {

            }
        }
    }
}
