# Unirest for .Net

![][unirest-logo]

[Unirest](http://unirest.io) is a set of lightweight HTTP libraries available in multiple languages, built and maintained by [Mashape](https://github.com/Mashape), who also maintain the open-source API Gateway [Kong](https://github.com/Mashape/kong). 

This is a port of the Java library to .NET. The synctax has been improved to implement C# coding standards.

## Installing
We're currently updating Nuget to point to the latest package.  In the meantime, please download this entire unirest-net library and reference it in your project.

## Creating Request
So you're probably wondering how using Unirest makes creating requests in .NET easier, here is a basic POST request that will explain everything:

```C#
HttpResponse<MyClass> jsonResponse = Unirest.Post("http://httpbin.org/post")
  .AddHeader("accept", "application/json")
  .AddField("parameter", "value")
  .AddField("foo", "bar")
  .AsJson<MyClass>();
```

Requests are made when `As[Type]()` is invoked, possible types include `Json`, `Binary`, `string`. If the request supports this, a body  can be passed along with `.SetBody(string)` or `SetBody<T>(T)` to serialize an arbitrary object to JSON. If you already have a dictionary of parameters or do not wish to use seperate field methods for each one there is a `.AddFields(Dictionary<string, object> parameters)` method that will serialize each key - value to form parameters on your request.

`.AddHeaders(Dictionary<string, string> headers)` is also supported in replacement of multiple header methods.

## Asynchronous Requests
Sometimes, well most of the time, you want your application to be asynchronous and not block, Unirest supports this in .NET with the TPL pattern and async/await:

```C#
Task<HttpResponse<MyClass>> myClassTask = Unirest.Post("http://httpbin.org/post")
  .AddHeader("accept", "application/json")
  .AddField("param1", "value1")
  .AddField("param2", "value2")
  .AsJsonAsync<MyClass>();
```

## File Uploads
Creating `multipart` requests with .NET is trivial, simply pass along a `Stream` Object as a field:

```C#
byte[] data = File.ReadAllBytes(@"filePath");
HttpResponse<MyClass> myClass = Unirest.Post("http://httpbin.org/post")
  .AddHeader("accept", "application/json")
  .AddField("parameter", "value")
  .AddField("files", data)
  .AAJson<MyClass>();
```

## Custom Entity Body

```C#
HttpResponse<MyClass> myClass = Unirest.Post("http://httpbin.org/post")
  .AddHeader("accept", "application/json")
  .SetBody("{\"parameter\":\"value\", \"foo\":\"bar\"}")
  .AsJson<MyClass>();
```

# Request

The .NET Unirest library follows the builder style conventions. You start building your request by creating a `HttpRequest` object using one of the following:

```C#
HttpRequest request = Unirest.Get(string url);
HttpRequest request = Unirest.Post(string url);
HttpRequest request = Unirest.Put(string url);
HttpRequest request = Unirest.Patch(string url);
HttpRequest request = Unirest.Delete(string url);
```

Uri objects are also accepted.

# Response

Upon recieving a response Unirest returns the result in the form of an Object, this object should always have the same keys for each language regarding to the response details.

- `.Code` - HTTP Response Status Code (Example 200)
- `.Headers` - HTTP Response Headers
- `.Body` - Parsed response body where applicable, for example JSON responses are parsed to Objects / Associative Arrays.
- `.Raw` - Un-parsed response body

## QueryString building

Adding query string parameters is a breeze. Portable solutions cannot rely on the HttpUtility.

```C#
HttpResponse<string> myClass = Unirest.Get("http://httpbin.org/html")
  .AddQueryStringParameter("test", "true")
  .AsString();
```

----

Made with &#9829; from the [Mashape](https://www.mashape.com/) team

[unirest-logo]: http://cl.ly/image/2P373Y090s2O/Image%202015-10-12%20at%209.48.06%20PM.png
