using System;
using System.Net.Http;

using unirest_net.request;

namespace unirest_net.http
{
    [Obsolete("Use UnirestNet.UnirestClient")]
    public class Unirest
    {
        [Obsolete("Use UnirestNet.UnirestClient.Get")]
        public static HttpRequest get(string url)
        {
            return new HttpRequest(HttpMethod.Get, url);
        }

        [Obsolete("Use UnirestNet.UnirestClient.Post")]
        public static HttpRequest post(string url)
        {
            return new HttpRequest(HttpMethod.Post, url);
        }

        [Obsolete("Use UnirestNet.UnirestClient.Delete")]
        public static HttpRequest delete(string url)
        {
            return new HttpRequest(HttpMethod.Delete, url);
        }

        [Obsolete("Use UnirestNet.UnirestClient.Patch")]
        public static HttpRequest patch(string url)
        {
            return new HttpRequest(new HttpMethod("PATCH"), url);
        }

        [Obsolete("Use UnirestNet.UnirestClient.Put")]
        public static HttpRequest put(string url)
        {
            return new HttpRequest(HttpMethod.Put, url);
        }
    }
}
