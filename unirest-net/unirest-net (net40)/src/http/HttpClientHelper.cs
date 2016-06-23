namespace UnirestNet.Http
{
    using Request;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Helper methods for the execution of requests. Helps to create the generic responses.
    /// </summary>
    public static class HttpClientHelper
    {
        private const string USER_AGENT_HEADER = "user-agent";
        private const string USER_AGENT = "unirest-net/1.1";

        /// <summary>
        /// Requests the specified request.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        public static HttpResponse<T> Request<T>(HttpRequest request)
        {
            var responseTask = RequestHelper(request);
            Task.WaitAll(responseTask);
            var response = responseTask.Result;

            return new HttpResponse<T>(response);
        }

        /// <summary>
        /// Executes the request asynchronously.
        /// </summary>
        /// <typeparam name="T">The generic type.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>The response task.</returns>
        public static Task<HttpResponse<T>> RequestAsync<T>(IHttpRequest request)
        {
            var responseTask = RequestHelper(request);
            return Task<HttpResponse<T>>.Factory.StartNew(() =>
            {
                Task.WaitAll(responseTask);
                return new HttpResponse<T>(responseTask.Result);
            });
        }

        /// <summary>
        /// Helps creating an async request. Makes sure the user agent is present.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response task with the response message.</returns>
        internal static Task<HttpResponseMessage> RequestHelper(IHttpRequest request)
        {
            if (!request.Headers.ContainsKey(USER_AGENT_HEADER))
            {
                request.Headers.Add(USER_AGENT_HEADER, USER_AGENT);
            }

            var client = new HttpClient();
            var msg = new HttpRequestMessage(request.HttpMethod, request.Uri);

            foreach (var header in request.Headers)
            {
                msg.Headers.Add(header.Key, header.Value);
            }

            if (request.Body.Any())
            {
                msg.Content = request.Body;
            }

            return client.SendAsync(msg);
        }
    }

    /// <summary>
    /// Interface that harmonizes both requests (current and legacy).
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// Gets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        MultipartFormDataContent Body { get;  }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        Dictionary<string, string> Headers { get;  }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        HttpMethod HttpMethod { get;  }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        Uri Uri { get;  }
    }
}

namespace unirest_net.http
{
    using request;
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    [Obsolete("Use UnirestNet.Http.HttpClientHelper")]
    public class HttpClientHelper
    {
        public static HttpResponse<T> Request<T>(HttpRequest request)
        {
            var responseTask = UnirestNet.Http.HttpClientHelper.RequestHelper(request);
            Task.WaitAll(responseTask);
            var response = responseTask.Result;

            return new HttpResponse<T>(response);
        }

        public static Task<HttpResponse<T>> RequestAsync<T>(HttpRequest request)
        {
            var responseTask = UnirestNet.Http.HttpClientHelper.RequestHelper(request);
            return Task<HttpResponse<T>>.Factory.StartNew(() =>
            {
                Task.WaitAll(responseTask);
                return new HttpResponse<T>(responseTask.Result);
            });
        }
    }
}