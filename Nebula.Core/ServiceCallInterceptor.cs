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
                var ex = rpc.ExecuteRemotely();

                try
                {
                    ex.Wait();
                }
                catch(Exception e)
                {
                    throw e.InnerException;
                }

                var resultType = invocation.Method.ReturnType;

                if (resultType.BaseType == typeof(Task) && resultType.GenericTypeArguments.Count() == 1)
                {
                    var underlyingType = resultType.GenericTypeArguments.FirstOrDefault();

                    var taskResultType = typeof(Task);
                    var fromResultMethod = taskResultType.GetMethod("FromResult").MakeGenericMethod(underlyingType);

                    object[] par = { ex.Result.Result };

                    var task = fromResultMethod.Invoke(null, par);

                    invocation.ReturnValue = task;
                }
                else
                if(resultType == typeof(Task) && resultType.GenericTypeArguments.Count() == 0)
                {
                    invocation.ReturnValue = ex;
                }
                else
                {
                    invocation.ReturnValue = ex.Result.Result;
                }
            }
        }
    }
}
