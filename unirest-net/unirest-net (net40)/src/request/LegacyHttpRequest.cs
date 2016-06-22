using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using unirest_net.http;

namespace unirest_net.request
{
    public class HttpRequest
    {
        private UnirestNet.Request.HttpRequest _request;

        public Uri URL
        {
            get { return _request.Uri; }
        }

        public HttpMethod HttpMethod
        {
            get { return _request.HttpMethod; }
        }

        public Dictionary<String, String> Headers
        {
            get { return _request.Headers; }
        }

        public MultipartFormDataContent Body
        {
            get { return _request.Body; }
        }

        public HttpRequest(HttpMethod method, string url)
        {
            _request = new UnirestNet.Request.HttpRequest(method, url);
        }

        public HttpRequest header(string name, string value)
        {
            Headers.Add(name, value);
            return this;
        }

        public HttpRequest headers(Dictionary<string, string> headers)
        {
            _request.AddHeaders(headers);
            return this;
        }

        public HttpRequest field(string name, string value)
        {
            _request.AddField(name, value);
            return this;
        }

        public HttpRequest field(string name, byte[] data)
        {
            _request.AddField(name, data);
            return this;
        }

        public HttpRequest field(Stream value)
        {
            _request.AddField(value);
            return this;
        }

        public HttpRequest fields(Dictionary<string, object> parameters)
        {
            _request.AddFields(parameters);
            return this;
        }

        public HttpRequest body(string body)
        {
            _request.SetBody(body);
            return this;
        }

        public HttpRequest body<T>(T body)
        {
            _request.SetBody(body);
            return this;
        }

        public HttpResponse<String> asString()
        {
            return HttpClientHelper.Request<string>(this);
        }

        public Task<HttpResponse<String>> asStringAsync()
        {
            return HttpClientHelper.RequestAsync<string>(this);
        }

        public HttpResponse<Stream> asBinary()
        {
            return HttpClientHelper.Request<Stream>(this);
        }

        public Task<HttpResponse<Stream>> asBinaryAsync()
        {
            return HttpClientHelper.RequestAsync<Stream>(this);
        }

        public HttpResponse<T> asJson<T>()
        {
            return HttpClientHelper.Request<T>(this);
        }

        public Task<HttpResponse<T>> asJsonAsync<T>()
        {
            return HttpClientHelper.RequestAsync<T>(this);
        }
    }
}