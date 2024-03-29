using System;
using System.Net;
using System.Text.RegularExpressions;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Yarp.Gateways
{
    public partial class DaprTransformProvider : ITransformProvider
    {
        public void Apply(TransformBuilderContext context)
        {
            var appId = context.Route.RouteId;
            if (appId == DaprConfigUtils.Auth || appId == DaprConfigUtils.Oss)
            {
                switch (context.Route.RouteId)
                {
                    case DaprConfigUtils.Auth:
                        context.AddRequestTransform(transformContext =>
                        {
                            var newPath =
                                transformContext.Path.Value?.AsSpan(DaprConfigUtils.Auth.Length + 1).ToString() ??
                                string.Empty;
                            var urlString = transformContext.Query.QueryString.HasValue
                                ? $"{transformContext.DestinationPrefix}/v1.0/invoke/{appId}/method{newPath}{transformContext.Query?.QueryString}"
                                : $"{transformContext.DestinationPrefix}/v1.0/invoke/{appId}/method{newPath}";
                            transformContext.ProxyRequest.RequestUri = new Uri(urlString);


                            return ValueTask.CompletedTask;
                        });
                        break;
                    case DaprConfigUtils.Oss:
                        context.AddRequestTransform(transformContext =>
                        {
                            return ValueTask.CompletedTask;
                        });
                        break;
                }


                //context.AddRequestTransform((transformContext) =>
                //{
                //    var appId = transformContext.DestinationPrefix;
                //    var newPath = transformContext.Path.Value ?? string.Empty;


                //    if (!string.IsNullOrEmpty(appId) && !string.IsNullOrEmpty(newPath))
                //    {
                //        var urlString = transformContext.Query.QueryString.HasValue
                //            ? $"{transformContext.DestinationPrefix}/v1.0/invoke/{appId}/method{newPath}{transformContext.Query?.QueryString}"
                //            : $"{transformContext.DestinationPrefix}/v1.0/invoke/{appId}/method{newPath}";
                //        transformContext.ProxyRequest.RequestUri = new Uri(urlString);

                //        return ValueTask.CompletedTask;
                //    }

                //    transformContext.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                //    transformContext.HttpContext.Response.WriteAsJsonAsync($"你所访问的路径:{transformContext.Path},不存在！");
                //    return ValueTask.CompletedTask;
                //});
            }
        }

        public void ValidateCluster(TransformClusterValidationContext context)
        {
            /*if (context.Cluster.ClusterId== DaprConfigUtils.DaprApiClusterId)
            {
            }*/
        }

        public void ValidateRoute(TransformRouteValidationContext context)
        {
            /*if (context.Route.RouteId == DaprConfigUtils.DaprApiRouteId)
            {
            }*/
        }
    }
}
