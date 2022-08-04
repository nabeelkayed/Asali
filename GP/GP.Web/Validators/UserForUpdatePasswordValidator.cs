using FluentValidation;
using GP.Core.Models;
using RealWord.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Web.Validators
{
    public class UserForUpdatePasswordValidator : AbstractValidator<UserForUpdatePasswordDto>
    {
        public UserForUpdatePasswordValidator()
        {
            RuleFor(u => u.NewPassword).NotEmpty()
                                    .MinimumLength(8)
                                    .MaximumLength(16)
                                    .Matches("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$")
                                    .WithMessage("regex error");
            RuleFor(u => u.OldPassword).NotEmpty()
                                    .MinimumLength(8)
                                    .MaximumLength(16)
                                    .Matches("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$")
                                    .WithMessage("regex error");
        }
    }
}