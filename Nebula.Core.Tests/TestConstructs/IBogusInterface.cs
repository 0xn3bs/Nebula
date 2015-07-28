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
        string ReturnBogusString(string input);
        Task<string> ReturnBogusStringAsync(string input);
        string ReturnBogusString(string input, long blah);

        GenericType ReturnBogusGenericObject<GenericType>();
        GenericType ReturnBogusGenericObject<GenericType>(GenericType input);
        GenericType ReturnBogusGenericObject<GenericType, GenericType2>(GenericType input, GenericType2 input2);

        bool ReturnBogusBool(BogusTrackable a);
    }
}
