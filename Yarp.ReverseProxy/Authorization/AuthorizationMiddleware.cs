using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;

namespace Yarp.Gateways.Authorization;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary />
    /// <param name="next"></param>
    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        bool allowsAnonymous = endpoint != null && endpoint.Metadata.Any(
                                   meta => meta is AllowAnonymousAttribute) ||
                               context.Request.Host.Value.StartsWith("127.0.0.1");

        var isAuthorized = allowsAnonymous || (context.User.Identity?.IsAuthenticated ?? false);

        if (!isAuthorized)
        {
            // 如果未授权，返回未授权的响应
            context.Response.StatusCode = 401; // Unauthorized
            return;
        }

        var userStore = context.RequestServices.GetService<UserStore>()!;

        //if (!allowsAnonymous && !userStore.CheckPermission(2, context.Request.Path))
        //{
        //    await context.ForbidAsync(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        //    return;
        //}

        // 如果授权成功，继续请求管道的下一个中间件
        await _next(context);
    }
}

// 扩展IApplicationBuilder接口以添加授权中间件
public static class AuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
    {
        return builder.Use(next =>
        {
            return async context =>
            {
                var middleware = new AuthorizationMiddleware(next);
                await middleware.InvokeAsync(context);
            };
        });
    }
}