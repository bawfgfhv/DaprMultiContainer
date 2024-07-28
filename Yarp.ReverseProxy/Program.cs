using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Yarp.Gateways;
using Yarp.Gateways.Authorization;

const string daprHost = "127.0.0.1";
const int daprPort = 3500;
const string daprInvokePath = "/v1.0/invoke/{appId}/method/";
const string corsPolicy = nameof(corsPolicy);

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddHttpForwarder();


var app = builder.Build();

var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
{
    UseProxy = false,
    AllowAutoRedirect = false,
    AutomaticDecompression = DecompressionMethods.None,
    UseCookies = false,
    ActivityHeadersPropagator = new ReverseProxyPropagator(DistributedContextPropagator.Current),
    ConnectTimeout = TimeSpan.FromSeconds(15),
});

var httpMessage = new HttpMessageInvoker(new HttpClientHandler()
{
    // 忽略https错误
    ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
    AllowAutoRedirect = false,
    AutomaticDecompression = DecompressionMethods.GZip,
    UseCookies = false,
    UseProxy = false,
    UseDefaultCredentials = true,
});

var transformer = new BingTransformer();
var requestConfig = new ForwarderRequestConfig { ActivityTimeout = TimeSpan.FromSeconds(100) };

app.UseCors(corsPolicy);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAuthorizationMiddleware();

//app.Map("/{appId}/api/connect/{**catch-all}",
//    async (HttpContext httpContext, IHttpForwarder forwarder, string appId) =>
//    {
//        var destinationUri =
//            $"http://{daprHost}:{daprPort}{daprInvokePath.Replace("{appId}", appId)}";

//        var error = await forwarder.SendAsync(httpContext, destinationUri, httpClient, requestConfig, transformer);

//        if (error != ForwarderError.None)
//        {
//            var errorFeature = httpContext.GetForwarderErrorFeature();
//            var exception = errorFeature.Exception;

//            // Handle the error or log it here.
//        }
//    }).AllowAnonymous();


app.Map("/{appId}/api/connect/{**catch-all}",[AllowAnonymous] async (HttpContext context, string appId) =>
{
    var destinationPrefix =
        $"http://{daprHost}:{daprPort}{daprInvokePath.Replace("{appId}", appId)}";

    var httpForwarder = context.RequestServices.GetRequiredService<IHttpForwarder>();
    await httpForwarder.SendAsync(context, destinationPrefix, httpMessage, new ForwarderRequestConfig(),
        transformer);
});

app.Map("/{appId}/{**catch-all}",
    async (HttpContext httpContext, IHttpForwarder forwarder, string appId) =>
    {
        var destinationUri =
            $"http://{daprHost}:{daprPort}{daprInvokePath.Replace("{appId}", appId)}";

        var error = await forwarder.SendAsync(httpContext, destinationUri, httpClient, requestConfig, transformer);

        if (error != ForwarderError.None)
        {
            var errorFeature = httpContext.GetForwarderErrorFeature();
            var exception = errorFeature.Exception;

            // Handle the error or log it here.
        }
    }).RequireAuthorization();


app.Run();