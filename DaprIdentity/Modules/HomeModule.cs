using Carter;
using Carter.ModelBinding;
using Carter.Request;
using Carter.Response;
using DaprIdentity.Authorization;
using DaprIdentity.Modules.User;

namespace DaprIdentity.Modules
{
    public class HomeModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.AppGet(Permissions.UserCreate, (HttpContext context) =>
            {
                return TypedResults.Ok(context.User);
            });

            app.AppPost("/bbq", (HttpContext context, UserInput userInput) =>
                {
                    var user = context.User;

                    return TypedResults.BadRequest(userInput);

                    //            return $"Hello {(user.GetClaim(Claims.Subject) ?? "lady gaga")} from Carter!";
                }).WithTags("Home 嘿嘿嘿嘿")
                .WithMetadata("meta---- 哈哈哈哈")
                .AllowAnonymous();


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
        public static RouteHandlerBuilder AppGet(this IEndpointRouteBuilder app, string pattern, Delegate handle)
        {
            return app.MapGet(pattern, handle).RequireAuthorization();
        }

        public static RouteHandlerBuilder AppPost(this IEndpointRouteBuilder app, string pattern, Delegate handle)
        {
            return app.MapPost(pattern, handle).RequireAuthorization();
        }

        public static RouteHandlerBuilder AppDelete(this IEndpointRouteBuilder app, string pattern, Delegate handle)
        {
            return app.MapDelete(pattern, handle).RequireAuthorization();
        }

        public static RouteHandlerBuilder AppPatch(this IEndpointRouteBuilder app, string pattern, Delegate handle)
        {
            return app.MapPatch(pattern, handle).RequireAuthorization();
        }

        public static RouteHandlerBuilder AppPut(this IEndpointRouteBuilder app, string pattern, Delegate handle)
        {
            return app.MapPut(pattern, handle).RequireAuthorization();
        }
    }
}