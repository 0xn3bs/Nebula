using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    public class Session : ISession
    {
        private readonly IWorkingSet _workingSet = new WorkingSet();

        public IWorkingSet WorkingSet
        {
            get { return _workingSet; }
        }
    }
}
