using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Amazon.Lambda.APIGatewayEvents
{
    public static class APIGatewayExtensions
    {
        public static object GetMainProperties(this APIGatewayHttpApiV2ProxyRequest proxy) => new
        {
            proxy.Body,
            proxy.RawQueryString,
            proxy.RawPath
        };
    }
}