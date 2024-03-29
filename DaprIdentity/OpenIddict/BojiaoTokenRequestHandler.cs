
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Collections.Immutable;
using System.Security.Claims;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace DaprIdentity.OpenIddict
{
    public class BojiaoTokenRequestHandler : IOpenIddictServerHandler<HandleTokenRequestContext>
    {
        public ValueTask HandleAsync(HandleTokenRequestContext context)
        {
            // 检查请求是否包含必要的参数（grant_type、username、password）
            if (!context.Request.IsPasswordGrantType() ||
                !context.Request.HasParameter("username") ||
                !context.Request.HasParameter("password"))
            {
                context.Reject(
                    error: Errors.InvalidRequest,
                    description: "The mandatory 'grant_type', 'username' and 'password' parameters are missing.");

                return default;
            }

            // 获取用户名和密码
            var username = context.Request.GetParameter("username");
            var password = context.Request.GetParameter("password");

            // 使用ASP.NET Core Identity验证用户名和密码
            //var result = await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: false);

            //if (!result.Succeeded)
            //{
            //context.Reject(
            //    error: Errors.InvalidGrant,
            //    description: "The username or password is incorrect.");

            //return default;
            //}

            // 验证成功，创建和签发access token
            var identity = new ClaimsIdentity(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme,
                Claims.Name, Claims.Role);
            identity.AddClaim(Claims.Subject, username.ToString())

                .SetClaim(OpenIddictConstants.Claims.Email, "jack@qq.com")
                .SetClaim(OpenIddictConstants.Claims.Name, "杰克")
                .SetClaims(OpenIddictConstants.Claims.Role,
                    ImmutableArray.Create<string>("Administrators", "Teachers", "Students"));

            var scopes = context.Request.GetScopes();

            //此除应与应用分配一致
            identity.SetScopes(scopes);

            identity.SetAudiences("resource_server_1");

            var principal = new ClaimsPrincipal(identity);
            context.SignIn(principal);
            return default;
        }
    }
}
