namespace UnirestNet.Http
{
    using System;
    using System.Net.Http;
    using UnirestNet.Request;

    /// <summary>
    /// Entry point for creating HTTP requests.
    /// </summary>
    public class Unirest
    {
        /// <summary>
        /// Creates a GET using the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A request.</returns>
        public static HttpRequest Get(string url)
        {
            return new HttpRequest(HttpMethod.Get, url);
        }

        /// <summary>
        /// Creates a GET using the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A request.</returns>
        public static HttpRequest Get(Uri uri)
        {
            return new HttpRequest(HttpMethod.Get, uri);
        }

        /// <summary>
        /// Creates a POST using the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A request.</returns>

        public static HttpRequest Post(string url)
        {
            return new HttpRequest(HttpMethod.Post, url);
        }

        /// <summary>
        /// Creates a POST using the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A request.</returns>

        public static HttpRequest Post(Uri uri)
        {
            return new HttpRequest(HttpMethod.Post, uri);
        }

        /// <summary>
        /// Creates a DELETE using the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A request.</returns>

        public static HttpRequest Delete(string url)
        {
            return new HttpRequest(HttpMethod.Delete, url);
        }

        /// <summary>
        /// Creates a DELETE using the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A request.</returns>
        public static HttpRequest Delete(Uri uri)
        {
            return new HttpRequest(HttpMethod.Get, uri);
        }

        /// <summary>
        /// Creates a PATCH using the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A request.</returns>
        public static HttpRequest Patch(string url)
        {
            return new HttpRequest(new HttpMethod("PATCH"), url);
        }

        /// <summary>
        /// Creates a PATCH using the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A request.</returns>
        public static HttpRequest Patch(Uri uri)
        {
            return new HttpRequest(new HttpMethod("PATCH"), uri);
        }

        /// <summary>
        /// Creates a PUT using the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A request.</returns>
        public static HttpRequest Put(string url)
        {
            return new HttpRequest(HttpMethod.Put, url);
        }

        /// <summary>
        /// Creates a PUT using the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A request.</returns>
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