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

    /// <summary>
    /// Models an HTTP request.
    /// </summary>
    public class HttpRequest : UnirestNet.Http.IHttpRequest
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

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URI.</param>
        /// <exception cref="System.ArgumentException">The url passed to the HttpMethod constructor is not a valid HTTP/S URL.</exception>
        public HttpRequest(HttpMethod method, Uri url)
        {
            if (!(url.IsAbsoluteUri && (url.Scheme == "http" || url.Scheme == "https")) || !url.IsAbsoluteUri)
            {
                throw new ArgumentException("The url passed to the HttpMethod constructor is not a valid HTTP/S URL.");
            }

            _uri = url;
            _httpMethod = method;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="url">The URL.</param>
        public HttpRequest(HttpMethod method, string url) : this(method, CreateUri(url))
        {
        }

        /// <summary>
        /// Executes the request and reads the response back as a binary stream.
        /// </summary>
        /// <returns>The response.</returns>
        public HttpResponse<Stream> AsBinary()
        {
            return HttpClientHelper.Request<Stream>(this);
        }

        /// <summary>
        /// Executes the request and reads the response back as a binary stream asynchronously.
        /// </summary>
        /// <returns>The response.</returns>
        public Task<HttpResponse<Stream>> AsBinaryAsync()
        {
            return HttpClientHelper.RequestAsync<Stream>(this);
        }

        /// <summary>
        /// Executes the request and reads the response back as a generic response object created using JSON deserialization.
        /// </summary>
        /// <returns>The response.</returns>
        public HttpResponse<T> AsJson<T>()
        {
            return HttpClientHelper.Request<T>(this);
        }

        /// <summary>
        /// Executes the request and returns a dynamic JSON object.
        /// </summary>
        /// <returns></returns>
        public dynamic AsJson()
        {
            var response = this.AsJson<object>();
            return GetJsonObject(response);
        }

        /// <summary>
        /// Executes the request and reads the response back as a generic response object created using JSON deserialization asynchronously.
        /// </summary>
        /// <returns>The response.</returns>
        public Task<HttpResponse<T>> AsJsonAsync<T>()
        {
            return HttpClientHelper.RequestAsync<T>(this);
        }

        /// <summary>
        /// Executes the request and reads the response back as a string created using JSON deserialization.
        /// </summary>
        /// <returns>The response.</returns>
        public HttpResponse<String> AsString()
        {
            return HttpClientHelper.Request<String>(this);
        }

        /// <summary>
        /// Executes the request and reads the response back as a string created using JSON deserialization asynchronously.
        /// </summary>
        /// <returns>The response.</returns>

        public Task<HttpResponse<String>> AsStringAsync()
        {
            return HttpClientHelper.RequestAsync<String>(this);
        }

        /// <summary>
        /// Sets the body.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <returns>The request for chaining.</returns>
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

        /// <summary>
        /// Sets the body. It will be serialized into JSON.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="body">The body.</param>
        /// <returns>The request for chaining.</returns>
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

        /// <summary>
        /// Adds the field.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>The request for chaining.</returns>
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

        /// <summary>
        /// Adds the field.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="data">The data.</param>
        /// <param name="mimeType">Type of the MIME (optional).</param>
        /// <param name="fileName">Name of the file (optional).</param>
        /// <returns>The request for chaining.</returns>
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

        /// <summary>
        /// Adds the field.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="bufferSize">Size of the buffer (optional).</param>
        /// <returns>The request for chaining.</returns>
        public HttpRequest AddField(Stream value, int bufferSize = 1024 * 8)
        {
            if (_httpMethod == System.Net.Http.HttpMethod.Get)
            {
                throw new InvalidOperationException(CannotAddBodyToGetRequestExceptionMessage);
            }

            if (_hasExplicitBody)
            {
                throw new InvalidOperationException(CannotAddFieldsToRequestWithExplicitBodyExceptionMessage);
            }

            _body.Add(new StreamContent(value, bufferSize));
            _hasFields = true;
            return this;
        }

        /// <summary>
        /// Adds the fields.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The request for chaining.</returns>
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

        /// <summary>
        /// Adds the query string parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value. Use <c>nulll</c> for unary parameters.</param>
        /// <returns>The request for chaining.</returns>
        public HttpRequest AddQueryStringParameter(string name, string value = null)
        {
            var dictionary = new Dictionary<string, string>() {
                {name, value}
            };

            return AddQueryStringParameters(dictionary);
        }

        /// <summary>
        /// Adds the query string parameters.
        /// </summary>
        /// <returns>The request for chaining.</returns>
        public HttpRequest AddQueryStringParameters(Dictionary<string, string> parameters)
        {
            string q = new FormUrlEncodedContent(parameters).ReadAsStringAsync().Result;

            var bob = new UriBuilder(_uri);
            if (bob.Query.Length > 1)
            {
                q = bob.Query.Substring(1) + q;
            }
            bob.Query = q;

            _uri = bob.Uri;

            return this;
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public MultipartFormDataContent Body
        {
            get { return _body; }
        }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public Dictionary<string, string> Headers
        {
            get { return _headers; }
        }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        /// <value>
        /// The HTTP method.
        /// </value>
        public HttpMethod HttpMethod
        {
            get { return _httpMethod; }
        }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public Uri Uri
        {
            get { return _uri; }
        }

        /// <summary>
        /// Adds the header.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>The request for chaining.</returns>
        public HttpRequest AddHeader(string name, string value)
        {
            _headers.Add(name, value);
            return this;
        }

        /// <summary>
        /// Adds the headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <returns>The request for chaining.</returns>
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

        /// <summary>
        /// Creates the URI.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The URI object.</returns>
        /// <exception cref="System.ArgumentException">The url passed to the HttpMethod constructor is not a valid HTTP/S URL.</exception>
        private static Uri CreateUri(string url)
        {
            Uri uri;

            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                throw new ArgumentException("The url passed to the HttpMethod constructor is not a valid HTTP/S URL.");
            }

            return uri;
        }

        /// <summary>
        /// Gets the json object.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>The json object.</returns>
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

    [Obsolete("Use UnirestNet.Request.HttpRequest")]
    public class HttpRequest : UnirestNet.Http.IHttpRequest
    {
        private UnirestNet.Request.HttpRequest _request;

        public Uri URL
        {
            get { return _request.Uri; }
        }

        Uri UnirestNet.Http.IHttpRequest.Uri { get { return _request.Uri; } }

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