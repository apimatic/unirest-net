namespace UnirestNet.Request
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using UnirestNet.Http;

    public class HttpRequest
    {
        private const string CannotAddBodyToGetRequestExceptionMessage = "Can't add body to Get request.";
        private const string CannotAddExplicitBodyToRequestWithFieldsExceptionMessage = "Can't add explicit body to request with fields";
        private const string CannotAddFieldsToRequestWithExplicitBodyExceptionMessage = "Can't add fields to a request with an explicit body";

        private MultipartFormDataContent _body = new MultipartFormDataContent();
        private bool _hasExplicitBody;
        private bool _hasFields;
        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        private HttpMethod _httpMethod;
        private Uri _uri;

        public HttpRequest(HttpMethod method, Uri uri)
        {
            if (!(uri.IsAbsoluteUri && (uri.Scheme == "http" || uri.Scheme == "https")) || !uri.IsAbsoluteUri)
            {
                throw new ArgumentException("The url passed to the HttpMethod constructor is not a valid HTTP/S URL");
            }

            _uri = uri;
            _httpMethod = method;
        }

        public HttpRequest(HttpMethod method, string url) : this(method, CreateUri(url))
        {
        }

        public HttpResponse<Stream> AsBinary()
        {
            return HttpClientHelper.Request<Stream>(this);
        }

        public Task<HttpResponse<Stream>> AsBinaryAsync()
        {
            return HttpClientHelper.RequestAsync<Stream>(this);
        }

        public HttpResponse<T> AsJson<T>()
        {
            return HttpClientHelper.Request<T>(this);
        }

        public dynamic AsJson()
        {
            var response = this.AsJson<object>();
            return GetJsonObject(response);
        }

        public Task<HttpResponse<T>> AsJsonAsync<T>()
        {
            return HttpClientHelper.RequestAsync<T>(this);
        }

        public HttpResponse<String> AsString()
        {
            return HttpClientHelper.Request<String>(this);
        }

        public Task<HttpResponse<String>> AsStringAsync()
        {
            return HttpClientHelper.RequestAsync<String>(this);
        }

        public HttpRequest SetBody(string body)
        {
            if (_httpMethod == System.Net.Http.HttpMethod.Get)
            {
                throw new InvalidOperationException(CannotAddBodyToGetRequestExceptionMessage);
            }

            if (_hasFields)
            {
                throw new InvalidOperationException(CannotAddExplicitBodyToRequestWithFieldsExceptionMessage);
            }

            _body = new MultipartFormDataContent { new StringContent(body) };
            _hasExplicitBody = true;
            return this;
        }

        public HttpRequest SetBody<T>(T body)
        {
            if (_httpMethod == System.Net.Http.HttpMethod.Get)
            {
                throw new InvalidOperationException(CannotAddBodyToGetRequestExceptionMessage);
            }

            if (_hasFields)
            {
                throw new InvalidOperationException(CannotAddExplicitBodyToRequestWithFieldsExceptionMessage);
            }

            _body = new MultipartFormDataContent { new StringContent(JsonConvert.SerializeObject(body)) };
            _hasExplicitBody = true;
            return this;
        }

        public HttpRequest AddField(string name, string value)
        {
            if (_httpMethod == System.Net.Http.HttpMethod.Get)
            {
                throw new InvalidOperationException(CannotAddBodyToGetRequestExceptionMessage);
            }

            if (_hasExplicitBody)
            {
                throw new InvalidOperationException(CannotAddFieldsToRequestWithExplicitBodyExceptionMessage);
            }

            _body.Add(new StringContent(value), name);

            _hasFields = true;
            return this;
        }

        public HttpRequest AddField(string name, byte[] data, string mimeType = "image/jpeg", string fileName = "image.jpg")
        {
            if (_httpMethod == System.Net.Http.HttpMethod.Get)
            {
                throw new InvalidOperationException(CannotAddBodyToGetRequestExceptionMessage);
            }

            if (_hasExplicitBody)
            {
                throw new InvalidOperationException(CannotAddFieldsToRequestWithExplicitBodyExceptionMessage);
            }

            var imageContent = new ByteArrayContent(data);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mimeType);

            _body.Add(imageContent, name, fileName);
            _hasFields = true;

            return this;
        }

        public HttpRequest AddField(Stream value)
        {
            if (_httpMethod == System.Net.Http.HttpMethod.Get)
            {
                throw new InvalidOperationException(CannotAddBodyToGetRequestExceptionMessage);
            }

            if (_hasExplicitBody)
            {
                throw new InvalidOperationException(CannotAddFieldsToRequestWithExplicitBodyExceptionMessage);
            }

            _body.Add(new StreamContent(value));
            _hasFields = true;
            return this;
        }

        public HttpRequest AddFields(Dictionary<string, object> parameters)
        {
            if (_httpMethod == System.Net.Http.HttpMethod.Get)
            {
                throw new InvalidOperationException(CannotAddBodyToGetRequestExceptionMessage);
            }

            if (_hasExplicitBody)
            {
                throw new InvalidOperationException(CannotAddFieldsToRequestWithExplicitBodyExceptionMessage);
            }

            _body.Add(new FormUrlEncodedContent(parameters.Where(kv => kv.Value is String).Select(kv => new KeyValuePair<string, string>(kv.Key, kv.Value as String))));

            foreach (var stream in parameters.Where(kv => kv.Value is Stream).Select(kv => kv.Value))
            {
                _body.Add(new StreamContent(stream as Stream));
            }

            _hasFields = true;
            return this;
        }

        public HttpRequest AddQueryStringParameter(string name, string value = null)
        {
            string q = new FormUrlEncodedContent(new Dictionary<string, string>() {
                {name, value}
            }).ReadAsStringAsync().Result;

            var bob = new UriBuilder(_uri);
            if (bob.Query.Length > 1)
            {
                q = bob.Query.Substring(1) + q;
            }
            bob.Query = q;

            _uri = bob.Uri;

            return this;
        }

        public MultipartFormDataContent Body
        {
            get { return _body; }
        }

        public Dictionary<string, string> Headers
        {
            get { return _headers; }
        }

        public HttpMethod HttpMethod
        {
            get { return _httpMethod; }
        }

        public Uri Uri
        {
            get { return _uri; }
        }

        public HttpRequest AddHeader(string name, string value)
        {
            _headers.Add(name, value);
            return this;
        }

        public HttpRequest AddHeaders(Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _headers.Add(header.Key, header.Value);
                }
            }

            return this;
        }

        private static Uri CreateUri(string url)
        {
            Uri uri;

            if (!System.Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                throw new ArgumentException("The url passed to the HttpMethod constructor is not a valid HTTP/S URL");
            }

            return uri;
        }

        private static dynamic GetJsonObject(HttpResponse<object> response)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(response.Raw))
            {
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize(jsonTextReader);
                }
            }
        }
    }
}

namespace unirest_net.request
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    using unirest_net.http;
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
