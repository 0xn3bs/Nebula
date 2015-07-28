using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq.Expressions;

namespace Nebula.Core
{
    public class ServiceCallInterceptor : IInterceptor
    {
        ISession _session;

        public ServiceCallInterceptor(ISession session)
        {
            _session = session;
        }

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
                if(_session == null)
                {
                    _session = new Session();
                }

                var rpc = new RemoteProcedureCall(_session, invocation);
                var ex = rpc.Execute();
                ex.Wait();

                var resultType = invocation.Method.ReturnType;

                if (resultType.BaseType == typeof(Task) && resultType.GenericTypeArguments.Count() > 0)
                {
                    var underlyingGenericType = resultType.GenericTypeArguments.FirstOrDefault();

                    var taskResultType = typeof(Task<>).MakeGenericType(underlyingGenericType);

                    var res = ex.Result;

                    var task = new Task<string>(() => { return (string)res; });

                    task.Start();
                    task.Wait();

                    invocation.ReturnValue = task;
                }
                else
                {
                    invocation.ReturnValue = ex.Result;
                }
            }
        }
    }
}
