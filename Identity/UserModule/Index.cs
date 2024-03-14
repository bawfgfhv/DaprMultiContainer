
namespace Identity.UserModule
{
    public static class Index
    {
        public static WebApplication Do(this WebApplication webApp)
        {
            webApp.MapPost("/api/hello", (HttpContext context, UserInfo userInfo) =>
            {
               // var userInfo1 = userInfo.Adapt<UserInfo>();

                var user = context.User;
                return
                    $"Hello {userInfo.FirstName}{userInfo.LastName},UserInfo{(ReferenceEquals(userInfo, userInfo) ? "等于" : "不等于")}UserInfo1!";
            });


            // 定义处理文件上传的路由
            webApp.MapPost("/upload", async (IFormFile file, ILogger<Program> logger) =>
            {
                //var form = await request.ReadFormAsync();

                //var file = form.Files[0];
                if (file.Length > 0)
                {
                    // 定义文件保存路径
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.FileName);

                    // 确保上传目录存在
                    var uploadDir = Path.GetDirectoryName(filePath)!;
                    Directory.CreateDirectory(uploadDir);

                    // 保存文件到服务器
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    logger.LogInformation($"File {file.FileName} uploaded successfully.");

                    // 返回成功消息
                    return Results.Ok("File uploaded successfully");
                }
                else
                {
                    // 返回错误消息
                    return Results.BadRequest("No file uploaded");
                }
            }).DisableAntiforgery();

            webApp.MapPost("/User/AddUser",
                (IUserService userService, UserInfo userInfo) => userService.AddUser(userInfo));

            return webApp;
        }

        public record UserInfo(int Id, string FirstName, string LastName, int Age);

        public static IServiceCollection AddUserService(this IServiceCollection services)
        {
            return services.AddTransient<IUserService, UserService>();
        }
    }
}
