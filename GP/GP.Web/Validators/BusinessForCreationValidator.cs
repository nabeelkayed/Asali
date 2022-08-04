using FluentValidation;
using RealWord.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Web.Validators
{
    public class BusinessForCreationValidator : AbstractValidator<BusinessForCreationDto>
    {
        public BusinessForCreationValidator()
        {
            RuleFor(b => b.FirstName).NotEmpty();
            RuleFor(b => b.LastName).NotEmpty();
            RuleFor(b => b.Email).NotEmpty()
                                 .EmailAddress()
                                 .WithMessage("Your Email should not be empty");
            RuleFor(b => b.Password).NotEmpty()
                                    .MinimumLength(8)
                                    .MaximumLength(16)
                                    .Matches("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$")
                                    .WithMessage("The password is not good");
            /*RuleFor(u => u.Username).NotEmpty()
                                    .Matches("^[a-zA-Z][a-zA-Z0-9._-]{0,21}([-.][^_]|[^-.]{2})$");*/
        }
    }
}