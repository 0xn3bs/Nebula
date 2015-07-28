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

        public async Task<string> ReturnBogusStringAsync(string input)
        {
            return await Task.Run(() => { return input; });
        }

        public GenericType ReturnBogusGenericObject<GenericType>()
        {
            return default(GenericType);
        }

        public GenericType ReturnBogusGenericObject<GenericType>(GenericType input)
        {
            return input;
        }

        public GenericType ReturnBogusGenericObject<GenericType, GenericType2>(GenericType input, GenericType2 input2)
        {
            return input;
        }

        public bool ReturnBogusBool(BogusTrackable a)
        {
            return true;
        }

        public async Task<bool> ReturnBogusBoolAsync(BogusTrackable a)
        {
            return await Task.Run(() => ReturnBogusBool(a));
        }
    }
}
