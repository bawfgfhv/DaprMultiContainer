using Yarp.ReverseProxy.Configuration;

namespace Yarp.Gateways
{
    public static partial class Extensions
    {
        public static WebApplicationBuilder AddYarpProxy(this WebApplicationBuilder builder)
        {
            builder.Services.AddReverseProxy()
                .LoadFromMemory(DaprConfigUtils.Routes, DaprConfigUtils.Clusters)
                .AddTransforms<DaprTransformProvider>();
            return builder;
        }
    }

    public partial class DaprConfigUtils
    {
        public const string Auth = "DaprIdentity";
        public const string AuthClusterId = nameof(AuthClusterId);
        public const string Oss = nameof(Oss);
        public const string OssClusterId = nameof(OssClusterId);

        public static RouteConfig[] Routes => new[]
        {
            new RouteConfig()
            {
                RouteId = Auth,
                ClusterId = AuthClusterId,
                AuthorizationPolicy = "anonymous",
                Match = new RouteMatch
                {
                    Path = "/DaprIdentity/{**catch-all}"
                }
            },
            new RouteConfig()
            {
                RouteId = Oss,
                ClusterId = OssClusterId,
                //AuthorizationPolicy = "anonymous",
                Match = new RouteMatch
                {
                    Path = "/oss/{**catch-all}"
                }
            }
        };

        public static ClusterConfig[] Clusters => new[]
        {
            new ClusterConfig()
            {
                ClusterId = AuthClusterId,
                Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                {
                    { "auth", new DestinationConfig() { Address = "http://127.0.0.1:3500" } },
                }
            },
            new ClusterConfig()
            {
                ClusterId = OssClusterId,
                Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
                {
                    { "oss", new DestinationConfig() { Address = "http://127.0.0.1:3500" } },
                }
            }
        };
    }
}
