using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nebula.Core
{
    public class ApplicationService
    {
        public async Task Host()
        {
            var appServicePrefix = ConfigurationManager.AppSettings.GetValues("ApplicationServicePrefix").FirstOrDefault();

            var listener = new HttpListener();

            listener.Prefixes.Add(appServicePrefix);
            listener.Start();

            while (true)
            {
                var context = await listener.GetContextAsync();

                string content = new System.IO.StreamReader(context.Request.InputStream).ReadToEnd();

                try
                {
                    var rpc = RemoteProcedureCall.Deserialize(content);
                    object result = rpc.Result();

                    var resultType = result.GetType();

                    if (resultType.BaseType == typeof(Task) && resultType.GenericTypeArguments.Count() > 0)
                    {
                        var taskResult = resultType.GetProperty("Result").GetValue(result);
                        result = taskResult;
                    }

                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.TypeNameHandling = TypeNameHandling.All;
                    settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
                    var serializedJson = JsonConvert.SerializeObject(result, settings);

                    byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(serializedJson);

                    System.IO.Stream output = context.Response.OutputStream;
                    output.Write(jsonBytes, 0, jsonBytes.Length);
                    output.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            listener.Stop();
        }
    }
}
