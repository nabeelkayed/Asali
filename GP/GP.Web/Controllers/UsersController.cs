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
            if (logedinUserToReturn != null)
            {
                return Ok(logedinUserToReturn);
            }

            //may be enter
            return NotFound("The email or the password is not correct");
        }

        [HttpGet("user")]
        public async Task<ActionResult<UserProfileDto>> GetCurrentUser()
        {
            var userToReturn = await _IUserService.GetCurrentUserAsync();
            if (userToReturn == null)
            {
                return Ok(userToReturn);
            }

            //will not enter becouse we don't call this api unless we loged in
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpGet("user/{username}")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(string username)
        {
            var userToReturn = await _IUserService.GetUserProfileAsync(username);
            if (userToReturn != null) 
            {
                return Ok(userToReturn);
            }

            //may be enter if the user name is not exist
            return NotFound("The user does not exist");
        }

        [AllowAnonymous]
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser(UserForCreationDto userForCreation)
        {
            bool userCreated = await _IUserService.CreateUserAsync(userForCreation);
            if (userCreated)
            {
                return Ok();
            }

            //will not enter
            return NotFound("The username or email is alredy used");
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateUser(UserForUpdateDto userForUpdate)
        {
            bool userUpdated = await _IUserService.UpdateUserAsync(userForUpdate);
            if (userUpdated) 
            {
                return Ok();
            }

            //will not enter
            return NotFound("The email dose not exist");
        }

        [HttpPut("user/password")]
        public async Task<IActionResult> UpdateUserPassword(UserForUpdatePasswordDto userForUpdatePassword)
        {
            bool userPasswordUpdated = await _IUserService.UpdateUserPasswordAsync(userForUpdatePassword);
            if (userPasswordUpdated)
            {
                return Ok();
            }

            //will not enter
            return NotFound("The old password in not correct");
        }
    }
}