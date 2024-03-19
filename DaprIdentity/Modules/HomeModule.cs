using Carter;
using Carter.ModelBinding;
using Carter.Request;
using Carter.Response;
using DaprIdentity.Modules.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace DaprIdentity.Modules
{
    public class HomeModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            //app.MapGet("/",
            //        //[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)] 
            //        async (HttpContext context) =>
            //        {
            //            var user = context.User;

            //            return $"Hello {(user.GetClaim(Claims.Subject) ?? "lady gaga")} from Carter!";
            //        })
            //    .WithTags("Home")
            //    .WithMetadata("meta")
            //    .RequireAuthorization();

            app.MapPost<UserInput>("/", async (HttpContext context, UserInput userInput) =>
                {
                    var user = context.User;

                    return $"Hello {(user.GetClaim(Claims.Subject) ?? "lady gaga")} from Carter!";
                }).WithTags("Home 嘿嘿嘿嘿")
                .WithMetadata("meta---- 哈哈哈哈");


            app.MapGet("/qs", (HttpRequest req) =>
            {
                var ids = req.Query.AsMultiple<int>("ids");
                return $"It's {string.Join(",", ids)}";
            }).WithTags("Home");

            app.MapGet("/conneg", (HttpResponse res) => res.Negotiate(new { Name = "Dave" }))
                .WithTags("Home").WithMetadata(" meta --- data -- 咳咳 ");

            app.MapPost("/validation", HandlePost)
                .WithTags("Home");
        }

        private IResult HandlePost(HttpContext ctx, Person person, IDatabase database)
        {
            var result = ctx.Request.Validate(person);

            if (!result.IsValid)
            {
                return Results.UnprocessableEntity(result.GetFormattedErrors());
            }

            var id = database.StorePerson(person);

            ctx.Response.Headers.Location = $"/{id}";
            return Results.StatusCode(201);
        }
    }

    public record Person(string Name);

    public interface IDatabase
    {
        int StorePerson(Person person);
    }

    public class Database : IDatabase
    {
        public int StorePerson(Person person)
        {
            //你的业务逻辑
            return 1;
        }
    }


    public static class EndpointRouteBuilderExtends
    {
        public static RouteHandlerBuilder Get(this IEndpointRouteBuilder app, string pattern, Delegate handle)
        {
            return app.MapGet(pattern, handle).RequireAuthorization();
        }
    }
}