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

                var rpc = new RemoteServiceCall(_session, invocation);
                var ex = rpc.ExecuteRemotely();

                try
                {
                    ex.Wait();
                }
                catch(Exception e) when (e.InnerException != null)
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
                if (rpc.ReturnType.IsGenericType && 
                    rpc.ReturnType.GetGenericTypeDefinition() == typeof(Nullable<>) && 
                    resultType.GenericTypeArguments.Count() == 1)
                {
                    object result = null;

                    var underlyingType = Nullable.GetUnderlyingType(rpc.ReturnType);

                    //  TODO: We need a better way of dealing with Guids.
                    //  Currently Guids are getting deserialized to strings which 
                    //  is problematic, this is a temporary fix that does not address all cases. 
                    //  A general fix will be addressed in 
                    //  https://github.com/inkadnb/Nebula/issues/3
                    //  Should also look into other value types that may have similar problems.

                    if (underlyingType == typeof(Guid) && 
                        ex.Result.Result != null && 
                        ex.Result.Result.GetType() == typeof(string))
                    {
                        Guid guid = new Guid();

                        if(Guid.TryParse((string)ex.Result.Result, out guid))
                        {
                            result = guid;
                        }  
                        else
                        {
                            throw new Exception("Unable to match result with service method return type");
                        }
                    }
                    
                    invocation.ReturnValue = result;
                }
                else
                {
                    invocation.ReturnValue = ex.Result.Result;
                }
            }
        }
    }
}
