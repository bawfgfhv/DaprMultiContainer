using FluentValidation;

namespace DaprIdentity.Modules
{
    public class UserInputValidator : AbstractValidator<Person>
    {
        public UserInputValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name cannot be empty.");
        }
    }
}