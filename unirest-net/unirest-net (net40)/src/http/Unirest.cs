namespace UnirestNet.Http
{
    using System;
    using System.Net.Http;
    using UnirestNet.Request;

    public class Unirest
    {
        public static HttpRequest Get(string url)
        {
            return new HttpRequest(HttpMethod.Get, url);
        }

        public static HttpRequest Get(Uri uri)
        {
            return new HttpRequest(HttpMethod.Get, uri);
        }

        public static HttpRequest Post(string url)
        {
            return new HttpRequest(HttpMethod.Post, url);
        }

        public static HttpRequest Post(Uri uri)
        {
            return new HttpRequest(HttpMethod.Post, uri);
        }

        public static HttpRequest Delete(string url)
        {
            return new HttpRequest(HttpMethod.Delete, url);
        }

        public static HttpRequest Delete(Uri uri)
        {
            return new HttpRequest(HttpMethod.Get, uri);
        }

        public static HttpRequest Patch(string url)
        {
            return new HttpRequest(new HttpMethod("PATCH"), url);
        }

        public static HttpRequest Patch(Uri uri)
        {
            return new HttpRequest(new HttpMethod("PATCH"), uri);
        }

        public static HttpRequest Put(string url)
        {
            return new HttpRequest(HttpMethod.Put, url);
        }

        public static HttpRequest Put(Uri uri)
        {
            return new HttpRequest(HttpMethod.Put, uri);
        }
    }
}


namespace unirest_net.http
{
    using System;
    using System.Net.Http;
    using unirest_net.request;

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