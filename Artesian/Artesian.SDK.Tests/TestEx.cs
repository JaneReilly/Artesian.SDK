using Flurl.Http.Testing;

using System.Collections.Generic;

namespace Artesian.SDK.Tests
{
	public static class TestEx
    {
        public static HttpCallAssertion WithHeadersTest(this HttpCallAssertion assertion)
        {
            return assertion
                    .WithHeader("Accept", "*application/x.msgpacklz4; q=1.0*")
                    .WithHeader("Accept", "*application/x-msgpack; q=0.75*")
                    .WithHeader("Accept", "*application/json; q=0.5*");
        }

        public static HttpCallAssertion ShouldHaveCalledPath(this HttpTest test, string path)
        {
            return test.ShouldHaveCalled(path + "*");
        }


        public static HttpCallAssertion WithQueryParamMultiple<T>(this HttpCallAssertion assertion, string name, IEnumerable<T> collection)
        {
            foreach (var i in collection)
                assertion = assertion.WithQueryParam(name, i);

            return assertion;
        }
    }
}
