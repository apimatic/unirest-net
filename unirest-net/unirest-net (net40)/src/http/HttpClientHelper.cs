namespace UnirestNet.Http
{
    using Request;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public static class HttpClientHelper
    {
        private const string USER_AGENT_HEADER = "user-agent";
        private const string USER_AGENT = "unirest-net/1.0";

        public static HttpResponse<T> Request<T>(HttpRequest request)
        {
            var responseTask = RequestHelper(request);
            Task.WaitAll(responseTask);
            var response = responseTask.Result;

            return new HttpResponse<T>(response);
        }

        public static Task<HttpResponse<T>> RequestAsync<T>(HttpRequest request)
        {
            var responseTask = RequestHelper(request);
            return Task<HttpResponse<T>>.Factory.StartNew(() =>
            {
                Task.WaitAll(responseTask);
                return new HttpResponse<T>(responseTask.Result);
            });
        }

        private static Task<HttpResponseMessage> RequestHelper(HttpRequest request)
        {
            if (!request.Headers.ContainsKey(USER_AGENT_HEADER))
            {
                request.AddHeader(USER_AGENT_HEADER, USER_AGENT);
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
        private const string USER_AGENT = "unirest-net/1.0";

        public static HttpResponse<T> Request<T>(HttpRequest request)
        {
            var responseTask = RequestHelper(request);
            Task.WaitAll(responseTask);
            var response = responseTask.Result;

            return new HttpResponse<T>(response);
        }

        public static Task<HttpResponse<T>> RequestAsync<T>(HttpRequest request)
        {
            var responseTask = RequestHelper(request);
            return Task<HttpResponse<T>>.Factory.StartNew(() =>
            {
                Task.WaitAll(responseTask);
                return new HttpResponse<T>(responseTask.Result);
            });
        }

        private static Task<HttpResponseMessage> RequestHelper(HttpRequest request)
        {
            if (!request.Headers.ContainsKey("user-agent"))
            {
                request.Headers.Add("user-agent", USER_AGENT);
            }

            var client = new HttpClient();
            var msg = new HttpRequestMessage(request.HttpMethod, request.URL);

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
}