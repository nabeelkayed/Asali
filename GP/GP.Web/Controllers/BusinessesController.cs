using GP.Core.Models;
using GP.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealWord.Core.Models;
using RealWord.Utils.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/businesses")]
    public class BusinessesController : ControllerBase
    {
        private readonly IBusinessService _IBusinessService;

        public BusinessesController(IBusinessService businessService)
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateBusiness(BusinessForCreationDto businessForCreationDto)
        {
            await _IBusinessService.CreateBusinessAsync(businessForCreationDto);//لازم يرجع انه نجحت العملية أو لا مع رسالة كيف ما بعرف 
            return Ok();
        }

        [HttpPut("setup")]
        public async Task<IActionResult> BusnissProfileSetup(BusinessProfileSetupDto businessProfileSetup)
        {
            await _IBusinessService.SetupBusinessProfile(businessProfileSetup);//الشركة ما بتبين الا لما يجهز البروفايل ونظهر اشي بدل على هيك
            return Ok();
        }

        [AllowAnonymous] //othintication optinal
        [HttpGet("{businessUsername}")]
        public async Task<ActionResult<BusinessProfileDto>> GetBusnissProfile(string businessUsername)
        {
            var businessToReturn = await _IBusinessService.GetBusinessProfileAsync(businessUsername);
            if (businessToReturn == null)
            {
                return NotFound();
            }

            return Ok(businessToReturn);
        }

        [HttpGet()]
        public async Task<ActionResult<BusinessProfileDto>> GetCurrentBusnissAsync()
        {
            var businessToReturn = await _IBusinessService.GetCurrentBusinessAsync();
            if (businessToReturn == null)
            {
                return NotFound();
            }

            return Ok(businessToReturn);
        }

        [AllowAnonymous] //othentication optinal
        [HttpGet("search")]
        public async Task<ActionResult<BusinessProfileDto>> GetBusinesses(BusinessesParameters businessesParameters)
        {
            var businessesToReturn = await _IBusinessService.GetBusinessesAsync(businessesParameters);
            if (businessesToReturn == null)
            {
                return NotFound();
            }

            return Ok(businessesToReturn);
        }

        [AllowAnonymous]//othentication optinal
        [HttpGet("{businessUsername}/photos")]
        public async Task<ActionResult<BusinessDto>> GetPhotosForBusiness(string businessUsername)
        {
            var photosToReturn = await _IBusinessService.GetPhotosForBusinessAsync(businessUsername);
            if (photosToReturn == null)
            {
                return NotFound();
            }

            return Ok(photosToReturn);
        }

        [HttpGet("{businessUsername}/followers")]
        public async Task<ActionResult<BusinessDto>> GetFollowersForBusiness(string businessUsername)
        {
            var followersToReturn = await _IBusinessService.GetFollowersForBusinessAsync(businessUsername);
            if (followersToReturn == null)
            {
                return NotFound();
            }

            return Ok(followersToReturn);
        }

        [HttpPut("{businessUsername}")]
        public async Task<IActionResult> UpdateBusinessProfileAsync(string businessUsername, BusinessProfileForUpdateDto businessProfileForUpdate)
        {
            var isBusinessExists = await _IBusinessService.BusinessExistsAsync(businessUsername);
            if (!isBusinessExists)
            {
                return NotFound();
            }

            var isAuthorized = await _IBusinessService.IsAuthorized(businessUsername);
            if (!isAuthorized)
            {
                return Forbid();
            }

            await _IBusinessService.UpdateBusinessProfileAsync(businessProfileForUpdate);
            return Ok();
        }

        [HttpPut("{businessUsername}")]
        public async Task<IActionResult> UpdateBusinessAsync(string businessUsername, BusinessForUpdateDto businessForUpdate)
        {
            var isBusinessExists = await _IBusinessService.BusinessExistsAsync(businessUsername);
            if (!isBusinessExists)
            {
                return NotFound();
            }

            var isAuthorized = await _IBusinessService.IsAuthorized(businessUsername);
            if (!isAuthorized)
            {
                return Forbid();
            }

            await _IBusinessService.UpdateBusinessAsync(businessForUpdate);
            return Ok();
        }

        [HttpPut("{businessUsername}/password")]
        public async Task<IActionResult> UpdateBusinessPassword(string businessUsername, BusinessForUpdatePasswordDto businessForUpdatePassword)
        {
            await _IBusinessService.UpdateBusinessPasswordAsync(businessForUpdatePassword);
            return Ok();
        }

        [HttpDelete("{businessUsername}")]
        public async Task<IActionResult> DeleteBusiness(string businessUsername)
        {
            var isBusinessExists = await _IBusinessService.BusinessExistsAsync(businessUsername);
            if (!isBusinessExists)
            {
                return NotFound();
            }

            var isAuthorized = await _IBusinessService.IsAuthorized(businessUsername);
            if (!isAuthorized)
            {
                return Forbid();
            }

            await _IBusinessService.DeleteBusinessAsync(businessUsername);
            return NoContent();
        }

        [HttpPost("{businessUsername}/follow")]
        public async Task<IActionResult> FollowBusiness(string businessUsername)
        {
            await _IBusinessService.FollowBusinessAsync(businessUsername);
            return Ok();
        }

        [HttpDelete("{businessUsername}/follow")]
        public async Task<IActionResult> UnfollowBusiness(string businessUsername)
        {
            await _IBusinessService.UnfollowBusinessAsync(businessUsername);
            return Ok();
        }

    }
}