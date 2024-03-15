using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Identity.Controllers.User
{
    public class UserFilterValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            //获取类的示例对象
            var user = (User)validationContext.ObjectInstance;
            if (String.IsNullOrWhiteSpace(user.userName))
                return new ValidationResult("用户名不能为空", new[] { nameof(user.userName) });

            if (user.userName.Length > 10)
                return new ValidationResult("用户名太长", new[] { nameof(user.userName) });

            if (!String.IsNullOrWhiteSpace(user.userMobile))
            {
                var regex = new Regex(@"^1[3456789]\d{9}$");
                if ((!regex.IsMatch(user.userMobile)))
                    return new ValidationResult("用户手机号格式不正确", new[] { nameof(user.userMobile) });
            }
            else
            {
                return new ValidationResult("用户手机号为空", new[] { nameof(user.userMobile) });
            }
            return ValidationResult.Success;
        }
    }
}
