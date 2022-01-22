using FluentValidation;

namespace EMS.Core.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .MinimumLength(6).WithMessage("Pasword must be at least 6 character")
                .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
                .Matches("[a-z]").WithMessage("Password must have atlest 1 lowercase character")
                .Matches("[0-9]").WithMessage("Password must contain a number")
                .Matches("^a-zA-Z0-9").WithMessage("Password must contain non alphanumeric");
        }
    }
}