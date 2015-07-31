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
                if (!listener.IsListening)
                    break;

                var context = await listener.GetContextAsync();

                string content = new System.IO.StreamReader(context.Request.InputStream).ReadToEnd();

                try
                {
                    object result = null;
                    RemoteServiceCall rpc = null;
                    RemoteServiceResponse response = null;
                    string json = null;

                    System.IO.Stream output = context.Response.OutputStream;

                    try
                    {
                        rpc = SerializationHelper.DeserializeFromJson<RemoteServiceCall>(content);
                        result = rpc.GetResult();

                        var resultType = result.GetType();

                        if (resultType.BaseType == typeof(Task) && resultType.GenericTypeArguments.Count() > 0)
                        {
                            var taskResult = resultType.GetProperty("Result").GetValue(result);
                            result = taskResult;
                        }
                    }
                    catch (Exception e)
                    {
                        response = new RemoteServiceResponse(null, e);

                        json = SerializeObjectToJson(response);

                        WriteJsonToOutputStream(output, json);
                    }

                    response = new RemoteServiceResponse(result, null);

                    json = SerializeObjectToJson(response);

                    WriteJsonToOutputStream(output, json);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);

                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "Application";
                        eventLog.WriteEntry(e.ToString(), EventLogEntryType.Warning);
                    }
                }
            }
        }

        private static void WriteJsonToOutputStream(System.IO.Stream output, string json)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);
            output.Write(jsonBytes, 0, jsonBytes.Length);
            output.Close();
        }

        private static string SerializeObjectToJson(object result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            var serializedJson = SerializationHelper.SerializeToJson(result);
            return serializedJson;
        }
    }
}
