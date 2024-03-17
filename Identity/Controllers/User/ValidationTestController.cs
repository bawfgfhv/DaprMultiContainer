using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Identity.Controllers.User
{
    //模型类
    public class User
    {
        [MaxLength(10, ErrorMessage = "名字太长了")]
        public string userName { get; set; }

        [RegularExpression(@"^1[3456789]\d{9}$", ErrorMessage = "请输入正确的手机号")]
        public string userMobile { get; set; }
    }

    [UserFilterValidation]
    public class User1
    {
        public string userName { get; set; }
        public string userMobile { get; set; }
    }

    //接口
    /// <inheritdoc />
    public class ValidationTestController : ControllerBase
    {
        [HttpPost("Add")]
        public Results<Ok<string>, ValidationProblem> AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                return TypedResults.Ok("添加成功");
            }

            var validProblem = ModelState.ToDictionary(x => x.Key,
                x => x.Value?.Errors.Select(y => y.ErrorMessage).ToArray() ?? Array.Empty<string>());
            return TypedResults.ValidationProblem(validProblem);
        }

        [HttpPost("Add1")]
        public Results<Ok<string>, ValidationProblem> AddUser(User1 user)
        {
            if (ModelState.IsValid)
            {
                return TypedResults.Ok("添加成功");
            }

            var validProblem = ModelState.ToDictionary(x => x.Key,
                x => x.Value?.Errors.Select(y => y.ErrorMessage).ToArray() ?? Array.Empty<string>());
            return TypedResults.ValidationProblem(validProblem);
        }
    }
}
