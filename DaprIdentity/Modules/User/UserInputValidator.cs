using FluentValidation;

namespace DaprIdentity.Modules.User
{
    public class UserInputValidator : AbstractValidator<UserInput>
    {
        public UserInputValidator()
        {
            RuleFor(p => p.userName).NotEmpty().WithMessage("用户名不能为空！");
            RuleFor(p => p.userMobile).NotEmpty().WithMessage("Name cannot be empty.");
        }
    }
}