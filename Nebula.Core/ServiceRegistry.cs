using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleInjector;

namespace Nebula.Core
{
    public static class ServiceRegistry
    {
        public static void Register<IInterface, CImplementation>()
            where IInterface : class
            where CImplementation : class, IInterface
        {
            try
            {
                _container.Register<IInterface, CImplementation>(Lifestyle.Transient);
                _container.Verify();
            }
            catch(Exception e)
            {

            }
        }

        public static IInterface GetInstance<IInterface>()
            where IInterface : class
        {
            
            return _container.GetInstance<IInterface>();
        }

        private static Container _container = new Container();
    }
}
