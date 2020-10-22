using Flurl.Http.Testing;

namespace Artesian.SDK.Tests
{
	public static class TestEx
    {
        public static HttpCallAssertion WithHeadersTest(this HttpCallAssertion assertion)
        {
            return assertion
                    .WithHeader("Accept", "application/x.msgpacklz4; q=1.0")
                    .WithHeader("Accept", "application/x-msgpack; q=0.75")
                    .WithHeader("Accept", "application/json; q=0.5");
        }
    }
}
