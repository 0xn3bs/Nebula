using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Nebula.Core
{
    public class ServiceCallInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var runLocallySetting = ConfigurationManager.AppSettings.GetValues("RunServicesLocally");

            var runLocally = true;

            if (runLocallySetting != null)
            {
                bool.TryParse(runLocallySetting.FirstOrDefault(), out runLocally);
            }

            if (runLocally)
            {
                invocation.Proceed();
            }
            else
            {
                //  Do remove service call
            }
        }
    }
}
