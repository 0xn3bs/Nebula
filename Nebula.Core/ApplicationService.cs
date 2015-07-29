﻿using Newtonsoft.Json;
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
                    RemoteProcedureCall rpc = null;
                    RemoteServiceResponse response = null;
                    byte[] jsonBytes = null;

                    try
                    {
                        rpc = RemoteProcedureCall.Deserialize(content);
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

                        jsonBytes = ObjectToJson(response);

                        WriteBytesToOutputStream(context, jsonBytes);

                        throw;
                    }

                    response = new RemoteServiceResponse(result, null);

                    jsonBytes = ObjectToJson(response);

                    WriteBytesToOutputStream(context, jsonBytes);
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

        private static void WriteBytesToOutputStream(HttpListenerContext context, byte[] jsonBytes)
        {
            System.IO.Stream output = context.Response.OutputStream;
            output.Write(jsonBytes, 0, jsonBytes.Length);
            output.Close();
        }

        private static byte[] ObjectToJson(object result)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            var serializedJson = JsonConvert.SerializeObject(result, settings);

            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(serializedJson);
            return jsonBytes;
        }
    }
}
