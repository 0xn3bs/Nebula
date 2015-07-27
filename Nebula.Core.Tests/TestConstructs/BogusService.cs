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

        public string ReturnBogusString(string input)
        {
            return input;
        }

        public string ReturnBogusString(string input, long blah)
        {
            return input;
        }

        public GenericType ReturnBogusGenericObject<GenericType>(GenericType input)
        {
            return input;
        }

        public GenericType ReturnBogusGenericObject<GenericType, GenericType2>(GenericType input, GenericType2 input2)
        {
            return input;
        }
    }
}
