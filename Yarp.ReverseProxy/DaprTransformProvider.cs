using System.Text.RegularExpressions;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Yarp.Gateways
{
    public partial class DaprTransformProvider : ITransformProvider
    {
        public void Apply(TransformBuilderContext context)
        {
            if (context.Route.RouteId == DaprConfigUtils.DaprUserApiRouteId ||
                context.Route.RouteId == DaprConfigUtils.DaprOssApiRouteId)
            {
                context.AddRequestTransform((transformContext) =>
                {
                    var path = transformContext.Path.Value ?? string.Empty;
                    if (Regex.IsMatch(path, @"^/app/"))
                    {
                        var index = path.IndexOf("/api/", StringComparison.Ordinal);
                        if (index != -1)
                        {
                            var appId = path.Substring(5, index - 5);
                            var newPath = path.Substring(index);
                            if (!string.IsNullOrEmpty(appId) && !string.IsNullOrEmpty(newPath))
                            {
                                string urlString = string.Empty;
                                    
                                if (transformContext.Query.QueryString.HasValue)
                                {
                                    urlString= $"{transformContext.DestinationPrefix}/v1.0/invoke/{appId}/method{newPath}{transformContext.Query?.QueryString}";
                                }
                                else
                                {
                                    urlString = $"{transformContext.DestinationPrefix}/v1.0/invoke/{appId}/method{newPath}";
                                }
                                transformContext.ProxyRequest.RequestUri = new Uri(urlString);
                                
                                return ValueTask.CompletedTask;
                            }
                        }
                    }

                    return ValueTask.FromException(new Exception($"路径错误,必须格式/app/appid/*,实际:{transformContext.Path}"));
                });
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
