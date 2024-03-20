using Carter;
using Carter.ModelBinding;
using Carter.Request;
using Carter.Response;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Server.AspNetCore;

namespace DaprIdentity.Modules
{
    public class HomeModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapGet("/", async (HttpContext context) =>
                {
                    var user = context.User;

                    return $"Hello {user.Identity?.Name ?? "lady gaga"} from Carter!";
                })
                .WithTags("Home")
                .WithMetadata("meta");
            app.MapGet("/qs", (HttpRequest req) =>
            {
                var ids = req.Query.AsMultiple<int>("ids");
                return $"It's {string.Join(",", ids)}";
            }).WithTags("Home");

            app.MapGet("/conneg", (HttpResponse res) => res.Negotiate(new { Name = "Dave" }))
                .WithTags("Home");

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
}