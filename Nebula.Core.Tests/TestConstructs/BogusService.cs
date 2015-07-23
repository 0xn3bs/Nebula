using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core.Tests.TestConstructs
{
    public class BogusService : IBogusServiceInterface
    {
        public string ReturnBogusString()
        {
            return "Bogus!";
        }
    }
}
