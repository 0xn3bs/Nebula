using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            return await Task.FromResult(ReturnBogusString(input));
        }

        public async Task DoSomethingAsync()
        {
            await Task.Run(() => { int i = 1; int j = i + 1; });
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

        public IBogusTrackable ReturnBogusObject(IBogusTrackable input)
        {
            return input;
        }

        public async Task<bool> ReturnBogusBoolAsync(BogusTrackable a)
        {
            return await Task.Run(() => ReturnBogusBool(a));
        }

        public void ThrowAnException(string exceptionMessage)
        {
            throw new Exception(exceptionMessage);
        }

        public string SlowServiceCall(string input)
        {
            Thread.Sleep(180000);
            return input;
        }

        public async Task<string> SlowServiceCallAsync(string input)
        {
            return await Task.FromResult(SlowServiceCall(input));
        }

        public Guid? ReturnNullableType(bool returnNull)
        {
            return returnNull ? (Guid?)null : Guid.NewGuid();
        }
    }
}
