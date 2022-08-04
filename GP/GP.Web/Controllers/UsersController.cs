using GP.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealWord.Core.Models;
using RealWord.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _IUserService;

        public UsersController(IUserService userService)
        {
            _IUserService = userService ??
               throw new ArgumentNullException(nameof(userService));
        }

        [AllowAnonymous]
        [HttpPost("users/login")]
        public async Task<ActionResult<UserDto>> UserLogin(UserLoginDto userLogin)
        {
            var logedinUserToReturn = await _IUserService.LoginUserAsync(userLogin);
            if (logedinUserToReturn == null)
            {
                return NotFound();
            }

            return Ok(logedinUserToReturn);
        }

        [HttpGet("user")]
        public async Task<ActionResult<UserProfileDto>> GetCurrentUser()
        {
            var userToReturn = await _IUserService.GetCurrentUserAsync();
            if (userToReturn == null)
            {
                return Unauthorized();
            }

            return Ok(userToReturn);
        }

        [AllowAnonymous]
        [HttpGet("user/{username}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(string username)
        {
            var userToReturn = await _IUserService.GetUserProfileAsync(username);
            if (userToReturn == null)
            {
                return Unauthorized();
            }

            return Ok(userToReturn);
        }

        [AllowAnonymous]
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser(UserForCreationDto userForCreation)
        {
            await _IUserService.CreateUserAsync(userForCreation);
            return Ok();
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateUser(UserForUpdateDto userForUpdate)
        {
            await _IUserService.UpdateUserAsync(userForUpdate);
            return Ok();
        }

        [HttpPut("user/password")]
        public async Task<IActionResult> UpdateUserPassword(UserForUpdatePasswordDto userForUpdatePassword)
        {
            await _IUserService.UpdateUserPasswordAsync(userForUpdatePassword);
            return Ok();
        }
    }
}