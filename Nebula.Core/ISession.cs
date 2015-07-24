using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    public interface ISession
    {
        IWorkingSet WorkingSet { get; }
    }
}
