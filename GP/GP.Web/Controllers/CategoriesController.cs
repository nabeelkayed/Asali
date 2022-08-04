using GP.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWord.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Web.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IBusinessService _IBusinessService;
        public CategoriesController(IBusinessService businessService)
        {
            _IBusinessService = businessService ??
                throw new ArgumentNullException(nameof(businessService));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<BusinessDto>> BusnissLogin(BusinessLoginDto businessLogin)
        {
            var logedinbusinessToReturn = await _IBusinessService.LoginBusinessAsync(businessLogin);
            if (logedinbusinessToReturn == null)
            {
                return NotFound();
            }

            return Ok(logedinbusinessToReturn);
        }
    }
}
