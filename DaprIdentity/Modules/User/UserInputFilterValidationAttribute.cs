using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DaprIdentity.Modules.User
{
    /// <inheritdoc />
    public class UserInputFilterValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            //获取类的示例对象
            var user = (UserInput)validationContext.ObjectInstance;
            if (string.IsNullOrWhiteSpace(user.UserName))
                return new ValidationResult("用户名不能为空", new[] { nameof(user.UserName) });

            if (user.UserName.Length > 10)
                return new ValidationResult("用户名太长", new[] { nameof(user.UserName) });

            if (!string.IsNullOrWhiteSpace(user.UserMobile))
            {
                var regex = new Regex(@"^1[3456789]\d{9}$");
                if (!regex.IsMatch(user.UserMobile))
                    return new ValidationResult("用户手机号格式不正确", new[] { nameof(user.UserMobile) });
            }
            else
            {
                return new ValidationResult("用户手机号为空", new[] { nameof(user.UserMobile) });
            }

            return ValidationResult.Success!;
        }
    }
}
