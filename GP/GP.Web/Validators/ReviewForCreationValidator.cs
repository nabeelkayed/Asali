using FluentValidation;
using RealWord.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Web.Validators 
{
    public class ReviewForCreationValidator : AbstractValidator<ReviewForCreationDto>
    {
        public ReviewForCreationValidator()
        {
            RuleFor(c => c.Body).NotEmpty();
            RuleFor(c => c.Rate).NotEmpty();
        }
    }
}
