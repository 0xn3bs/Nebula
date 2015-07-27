using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core.Tests.TestConstructs
{
    public interface IBogusServiceInterface
    {
        string ReturnBogusString();
        GenericType ReturnBogusGenericObject<GenericType>(GenericType input);
    }
}
