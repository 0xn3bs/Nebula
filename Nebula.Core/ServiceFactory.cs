using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleInjector;

namespace Nebula.Core
{
    //  Do we really want a singleton?
    public class ServiceFactory
    {
        private static ServiceFactory _instance;

        private ServiceFactory()
        {
            _container = new Container();
        }

        public static ServiceFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServiceFactory();
                }
                return _instance;
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
                TypeRegistry.Instance.Register(typeof(CImplementation));
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
