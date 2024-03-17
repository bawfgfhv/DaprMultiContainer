using FluentValidation;

namespace DaprIdentity.Modules
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name cannot be empty.");
        }
    }
}