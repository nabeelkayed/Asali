using FluentValidation;
using GP.Core.Models;
using RealWord.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Web.Validators
{
    public class BusinessForUpdateValidator : AbstractValidator<BusinessForUpdateDto>
    {
        public BusinessForUpdateValidator()
        {
           /* RuleFor(b => b.Email).NotEmpty();
            RuleFor(b => b.FirstName).NotEmpty();
            RuleFor(b => b.LastName).NotEmpty();
            RuleFor(b => b.Photo).NotEmpty();*/
        }
    }
}