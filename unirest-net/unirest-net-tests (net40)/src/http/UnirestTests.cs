namespace Unirest.Net.Http
{
    using FluentAssertions;
    using System.Net.Http;
    using UnirestNet.Http;

#if NETFX_CORE || WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    [TestClass]
    public class UnirestTest
    {
        [TestMethod]
        public void Unirest_Should_Return_Correct_Verb()
        {
            Unirest.Get("http://localhost").HttpMethod.Should().Be(HttpMethod.Get);
            Unirest.Post("http://localhost").HttpMethod.Should().Be(HttpMethod.Post);
            Unirest.Delete("http://localhost").HttpMethod.Should().Be(HttpMethod.Delete);
            Unirest.Patch("http://localhost").HttpMethod.Should().Be(new HttpMethod("PATCH"));
            Unirest.Put("http://localhost").HttpMethod.Should().Be(HttpMethod.Put);
        }

        [TestMethod]
        public void Unirest_Should_Return_Correct_URL()
        {
            Unirest.Get("http://localhost").Uri.OriginalString.Should().Be("http://localhost");
            Unirest.Post("http://localhost").Uri.OriginalString.Should().Be("http://localhost");
            Unirest.Delete("http://localhost").Uri.OriginalString.Should().Be("http://localhost");
            Unirest.Patch("http://localhost").Uri.OriginalString.Should().Be("http://localhost");
            Unirest.Put("http://localhost").Uri.OriginalString.Should().Be("http://localhost");
        }
    }
}


namespace unirest_net_tests.http
{
    using FluentAssertions;
    using System.Net.Http;
    using unirest_net.http;

#if NETFX_CORE || WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    [TestClass]
    public class UnirestTest
    {
        [TestMethod]
        public void Unirest_Should_Return_Correct_Verb()
        {
            Unirest.get("http://localhost").HttpMethod.Should().Be(HttpMethod.Get);
            Unirest.post("http://localhost").HttpMethod.Should().Be(HttpMethod.Post);
            Unirest.delete("http://localhost").HttpMethod.Should().Be(HttpMethod.Delete);
            Unirest.patch("http://localhost").HttpMethod.Should().Be(new HttpMethod("PATCH"));
            Unirest.put("http://localhost").HttpMethod.Should().Be(HttpMethod.Put);
        }

        [TestMethod]
        public void Unirest_Should_Return_Correct_URL()
        {
            Unirest.get("http://localhost").URL.OriginalString.Should().Be("http://localhost");
            Unirest.post("http://localhost").URL.OriginalString.Should().Be("http://localhost");
            Unirest.delete("http://localhost").URL.OriginalString.Should().Be("http://localhost");
            Unirest.patch("http://localhost").URL.OriginalString.Should().Be("http://localhost");
            Unirest.put("http://localhost").URL.OriginalString.Should().Be("http://localhost");
        }
    }
}
