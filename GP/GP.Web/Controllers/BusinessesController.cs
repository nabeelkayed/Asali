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
        public async Task<IActionResult> CreateBusiness([FromBody] BusinessForCreationDto businessForCreationDto)
        {
            bool createBusiness = await _IBusinessService.CreateBusinessAsync(businessForCreationDto);
            if (!createBusiness)
            {
                return NotFound("The Business is exist");
            }

            return Ok();
        }

        [HttpPut("setup")]
        public async Task<IActionResult> BusnissProfileSetup(BusinessProfileSetupDto businessProfileSetup)
        {
            bool setupBusinessProfile = await _IBusinessService.SetupBusinessProfileAsync(businessProfileSetup);//الشركة ما بتبين الا لما يجهز البروفايل ونظهر اشي بدل على هيك
            if (!setupBusinessProfile)
            {
                return NotFound();
            }

            return Ok();
        }

        [AllowAnonymous] //othintication optinal we should do it in maping
        [HttpGet("{businessUsername}")]
        public async Task<ActionResult<BusinessProfileDto>> GetBusinessProfile(string businessUsername)
        {
            var businessToReturn = await _IBusinessService.GetBusinessProfileAsync(businessUsername);
            if (businessToReturn == null)
            {
                return NotFound("The Business is not exist");
            }

            return Ok(businessToReturn);
        }

        [HttpGet()]
        public async Task<ActionResult<BusinessProfileDto>> GetCurrentBusinessAsync()
        {
            var businessToReturn = await _IBusinessService.GetCurrentBusinessAsync();
            if (businessToReturn == null)
            {
                return NotFound();
            }

            return Ok(businessToReturn);
        }

        [AllowAnonymous] //othentication optinal in the mabing we should do it
        [HttpGet("search")]
        public async Task<ActionResult<BusinessProfileDto>> GetBusinesses(BusinessesParameters businessesParameters)
        {
            var businessesToReturn = await _IBusinessService.GetBusinessesAsync(businessesParameters);
            if (businessesToReturn == null)
            {
                return NotFound("There is no businesses match your search");
            }

            return Ok(businessesToReturn);
        }

        [AllowAnonymous]//othentication optinal we should do it in maping
        [HttpGet("{businessId}/photos")]
        public async Task<ActionResult<BusinessDto>> GetPhotosForBusiness(Guid businessId)
        {
            var photosToReturn = await _IBusinessService.GetPhotosForBusinessAsync(businessId);
            if (photosToReturn == null)
            {
                return NotFound();
            }

            return Ok(photosToReturn);
        }

        [HttpGet("{businessId}/followers")]
        public async Task<ActionResult<BusinessDto>> GetFollowersForBusiness(Guid businessId)
        {
            var followersToReturn = await _IBusinessService.GetFollowersForBusinessAsync(businessId);
            if (followersToReturn == null)
            {
                return NotFound();
            }

            return Ok(followersToReturn);
        }

        [HttpPut(/*"{businessUsername}"*/"profile")]
        public async Task<IActionResult> UpdateBusinessProfileAsync(/*string businessUsername,*/ BusinessProfileForUpdateDto businessProfileForUpdate)
        {
            /*var isBusinessExists = await _IBusinessService.BusinessExistsAsync(businessUsername);
            if (!isBusinessExists)
            {
                return NotFound();
            }

            var isAuthorized = await _IBusinessService.IsAuthorized(businessUsername);
            if (!isAuthorized)
            {
                return Forbid();
            }*/

            bool updateBusinessProfile = await _IBusinessService.UpdateBusinessProfileAsync(businessProfileForUpdate);
            if (!updateBusinessProfile)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPut(/*"{businessUsername}"*/)]
        public async Task<IActionResult> UpdateBusinessAsync(/*string businessUsername,*/ BusinessForUpdateDto businessForUpdate)
        {
         /*   var isBusinessExists = await _IBusinessService.BusinessExistsAsync(businessUsername);
            if (!isBusinessExists)
            {
                return NotFound();
            }

            var isAuthorized = await _IBusinessService.IsAuthorized(businessUsername);
            if (!isAuthorized)
            {
                return Forbid();
            }*/

            bool updateBusiness = await _IBusinessService.UpdateBusinessAsync(businessForUpdate);
            if (!updateBusiness)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPut(/*"{businessUsername}/*/"password")]
        public async Task<IActionResult> UpdateBusinessPassword(/*string businessUsername,*/ BusinessForUpdatePasswordDto businessForUpdatePassword)
        {
            bool updateBusinessPassword = await _IBusinessService.UpdateBusinessPasswordAsync(businessForUpdatePassword);
            if (!updateBusinessPassword)
            {
                return NotFound();
            }

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

            bool deleteBusiness = await _IBusinessService.DeleteBusinessAsync(businessUsername);
            if (!deleteBusiness)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{businessUsername}/follow")]
        public async Task<IActionResult> FollowBusiness(string businessUsername)
        {
            bool followBusiness = await _IBusinessService.FollowBusinessAsync(businessUsername);
            if (!followBusiness)
            {
                return NotFound();

            }

            return Ok();
        }

        [HttpDelete("{businessUsername}/follow")]
        public async Task<IActionResult> UnfollowBusiness(string businessUsername)
        {
            bool unFollowBusiness = await _IBusinessService.UnfollowBusinessAsync(businessUsername);
            if (!unFollowBusiness)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}