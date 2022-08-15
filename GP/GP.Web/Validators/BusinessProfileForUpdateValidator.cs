using FluentValidation;
using GP.Core.Models;
using RealWord.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Web.Validators
{
    public class BusinessProfileForUpdateValidator : AbstractValidator<BusinessProfileForUpdateDto>
    {
        public BusinessProfileForUpdateValidator()
        {
            /*RuleFor(b => b.BusinessName).NotEmpty();
            RuleFor(b => b.Category).NotEmpty();
            RuleFor(b => b.MenuWebsite).NotEmpty();
            RuleFor(b => b.PhoneNumber).NotEmpty();
            RuleFor(b => b.Website).NotEmpty();
            RuleFor(b => b.Map).NotEmpty();
            RuleFor(b => b.Location).NotEmpty();
            RuleFor(b => b.Bio).NotEmpty();*/
        }
    }
}