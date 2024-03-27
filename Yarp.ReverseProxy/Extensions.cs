using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using Yarp.Gateways;
using Yarp.ReverseProxy.Configuration;
using Microsoft.AspNetCore.Authorization;

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
        public const string DaprUserApiRouteId = "dapr-user-service";
        public const string UserClusterId = "dapr-user";
        public const string DaprOssApiRouteId = "dapr-oss-service";
        public const string OssClusterId = "dapr-oss";
        public const string DaprApiStartBy = "/app/";

        public static RouteConfig[] Routes => new[]
        {
            new RouteConfig()
            {
                RouteId = DaprUserApiRouteId,
                ClusterId = UserClusterId,
                AuthorizationPolicy = "anonymous",
                Match = new RouteMatch
                {
                    Path = DaprApiStartBy + "auth/{**catch-all}"
                }
            },         
            new RouteConfig()
            {
                RouteId = DaprOssApiRouteId,
                ClusterId = OssClusterId,
                //AuthorizationPolicy = "anonymous",
                Match = new RouteMatch
                {
                    Path = DaprApiStartBy + "oss/{**catch-all}"
                }
            }
        };

        public static ClusterConfig[] Clusters => new[]
        {
            new ClusterConfig()
            {
                ClusterId = UserClusterId,
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
