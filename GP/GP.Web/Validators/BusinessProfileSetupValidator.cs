using FluentValidation;
using GP.Core.Models;
using RealWord.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Web.Validators
{
    public class BusinessProfileSetupValidator : AbstractValidator<BusinessProfileSetupDto>
    {
        public BusinessProfileSetupValidator()
        {
            RuleFor(b => b.BusinessName).NotEmpty();
            RuleFor(b => b.Category).NotEmpty();
            RuleFor(b => b.MenuWebsite).NotEmpty();
            RuleFor(b => b.PhoneNumber).NotEmpty();
            RuleFor(b => b.Website).NotEmpty();
        }
    }
}