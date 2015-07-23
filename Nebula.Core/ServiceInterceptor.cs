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
        public void Intercept(IInvocation invocation)
        {
            Debug.WriteLine(">> intercepted in <<");

            invocation.Proceed();

            invocation.ReturnValue = "BogusAltered!";

            Debug.WriteLine(">> intercepted out <<");
        }
    }
}
