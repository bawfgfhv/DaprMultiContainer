using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using Yarp.Gateways;
using Yarp.Gateways.Authorization;


const string corsPolicy = nameof(corsPolicy);

var builder = WebApplication.CreateBuilder(args);

builder.AddYarpProxy();

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.SetIssuer("http://192.168.1.10:5112");
        options.AddAudiences("resource_server_1");

        options.UseIntrospection()
            .SetClientId("resource_server_1")
            .SetClientSecret("846B62D0-DEF9-4215-A99D-86E6B8DAB342");

        options.AddEncryptionKey(new SymmetricSecurityKey(
            Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));


        options.UseSystemNetHttp();

        options.UseAspNetCore();
    });





// 配置跨域处理，允许所有来源
builder.Services.AddCors(options => options.AddPolicy(corsPolicy,
    policyBuilder =>
    {
        policyBuilder.AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowCredentials();
    }));

builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();


var app = builder.Build();

app.UseCors(corsPolicy);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorizationMiddleware();


app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Gateway started successfully!");
}).AllowAnonymous();

app.MapReverseProxy().RequireAuthorization();

await app.RunAsync();
