using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleInjector;

namespace Nebula.Core
{
    public class ServiceRegistry
    {
        private static ServiceRegistry instance;

        private ServiceRegistry()
        {
            _container = new Container();
        }

        public static ServiceRegistry Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServiceRegistry();
                }
                return instance;
            }
        }

        public void Register<IInterface, CImplementation>()
            where IInterface : class
            where CImplementation : class, IInterface
        {
            var registrations = _container.GetCurrentRegistrations();

            bool hasReg = registrations.Any(x => x.ServiceType == typeof(IInterface));

            if (!hasReg)
            {
                _container.Register<IInterface, CImplementation>(Lifestyle.Transient);
                _container.Verify();
            }
        }

        public IInterface GetInstance<IInterface>()
            where IInterface : class
        {
            return _container.GetInstance<IInterface>();
        }

        private Container _container;
    }
}
