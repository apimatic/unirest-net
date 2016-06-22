﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UnirestNet.Http
{
    public class HttpResponse<T>
    {
        public int Code { get; private set; }

        public Dictionary<string, string> Headers { get; private set; }

        public T Body { get; set; }

        public Stream Raw { get; private set; }

        public HttpResponse(HttpResponseMessage response)
        {
            Headers = new Dictionary<string, string>();
            Code = (int)response.StatusCode;

            if (response.Content != null)
            {
                var streamTask = response.Content.ReadAsStreamAsync();
                Task.WaitAll(streamTask);
                Raw = streamTask.Result;

                if (typeof(T) == typeof(string))
                {
                    var stringTask = response.Content.ReadAsStringAsync();
                    Task.WaitAll(stringTask);
                    Body = (T)(object)stringTask.Result;
                }
                else if (typeof(Stream).IsAssignableFrom(typeof(T)))
                {
                    Body = (T)(object)Raw;
                }
                else
                {
                    var stringTask = response.Content.ReadAsStringAsync();
                    Task.WaitAll(stringTask);
                    Body = JsonConvert.DeserializeObject<T>(stringTask.Result);
                }
            }

            foreach (var header in response.Headers)
            {
                Headers.Add(header.Key, header.Value.First());
            }
        }
    }
}