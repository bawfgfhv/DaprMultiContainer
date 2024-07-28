using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Transforms;

namespace Yarp.Gateways;

class BojiaoTransformer : HttpTransformer
{
    public override async ValueTask TransformRequestAsync(HttpContext httpContext,
        HttpRequestMessage proxyRequest, string destinationPrefix, CancellationToken cancellationToken)
    {
        // Copy all request headers
       // await base.TransformRequestAsync(httpContext, proxyRequest, destinationPrefix, cancellationToken);

        // Customize the query string:
        var queryContext = new QueryTransformContext(httpContext.Request);
        //queryContext.Collection.Remove("param1");
        queryContext.Collection["area"] = "xx2";

        var path = httpContext.Request.Path.Value?.Split('/').Skip(2).Aggregate((a, b) => $"{a}/{b}");

        // Assign the custom uri. Be careful about extra slashes when concatenating here. RequestUtilities.MakeDestinationAddress is a safe default.
        proxyRequest.RequestUri = RequestUtilities.MakeDestinationAddress(destinationPrefix, $"/{path}"
            , queryContext.QueryString);

        // Suppress the original request header, use the one from the destination Uri.
        proxyRequest.Headers.Host = null;
    }
}