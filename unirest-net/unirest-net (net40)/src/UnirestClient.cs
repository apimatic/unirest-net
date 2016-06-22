using System;
using System.Net.Http;
using UnirestNet.Request;

namespace UnirestNet
{
    public class UnirestClient
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
