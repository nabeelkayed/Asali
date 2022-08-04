﻿using AutoMapper;
using GP.Core.Models;
using Microsoft.AspNetCore.Http;
using RealWord.Core.Auth;
using RealWord.Core.Models;
using RealWord.Data.Entities;
using RealWord.Data.Repositories;
using RealWord.Utils.Utils;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealWord.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _IUserRepository;
        private readonly IUserAuth _IUserAuth;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IUserAuth userAuth,
            IHttpContextAccessor accessor, IMapper mapper)
        {
            _IUserRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));
            _IUserAuth = userAuth ??
                throw new ArgumentNullException(nameof(userAuth));
            _accessor = accessor ??
                throw new ArgumentNullException(nameof(accessor));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<UserDto> LoginUserAsync(UserLoginDto userLogin)
        {
            userLogin.Email = userLogin.Email.ToLower();
            userLogin.Password = userLogin.Password.GetHash();

            var user = _mapper.Map<User>(userLogin);
            var userlogedin = await _IUserRepository.LoginUserAsync(user);
            if (userlogedin == null)
            {
                return null;
            }

            var userToReturn = _mapper.Map<UserDto>(userlogedin);//يجب التأكد
            userToReturn.Token = _IUserAuth.Generate(userlogedin);
            return userToReturn;
        }

        public async Task<UserProfileDto> GetCurrentUserAsync()
        {
            var currentUsername = _accessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!String.IsNullOrEmpty(currentUsername))
            {
                var currentUser = await _IUserRepository.GetUserAsNoTrackingAsync(currentUsername);
                var userToReturn = _mapper.Map<UserProfileDto>(currentUser);//جيب التأكد
                return userToReturn;
            }

            return null;
        }

        public async Task<Guid> GetCurrentUserIdAsync()
        {
            var currentUsername = _accessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!String.IsNullOrEmpty(currentUsername))
            {
                var currentUser = await _IUserRepository.GetUserAsNoTrackingAsync(currentUsername);
                return currentUser.UserId;
            }

            return Guid.Empty;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string username)
        {
            username = username.ToLower();

            var user = await _IUserRepository.GetUserAsync(username);
            if (user == null)
            {
                return null;
            }

            var currentUserId = await GetCurrentUserIdAsync();

            var userProfileToReturn = _mapper.Map<UserProfileDto>(user, a => a.Items["currentUserId"] = currentUserId);//يجب التأكد
            return userProfileToReturn;
        }

        public async Task CreateUserAsync(UserForCreationDto userForCreation)
        {
            userForCreation.Username = userForCreation.Username.ToLower();
            userForCreation.Email = userForCreation.Email.ToLower();
            userForCreation.Password = userForCreation.Password.GetHash();

            var userExists = await _IUserRepository.UserExistsAsync(userForCreation.Username);
            if (userExists)
            {
                // return null;//return a messaage to till that the user name is used
            }

            var emailNotAvailable = await _IUserRepository.EmailAvailableAsync(userForCreation.Email);
            if (emailNotAvailable)
            {
                // return null;//return a messaage to till that the email is used
            }

            var userEntityForCreation = _mapper.Map<User>(userForCreation);// يجب التأكد
            await _IUserRepository.CreateUserAsync(userEntityForCreation);
            await _IUserRepository.SaveChangesAsync();

            //    var createdUserToReturn = _mapper.Map<UserDto>(userEntityForCreation);
            //  return createdUserToReturn;
        }

        public async Task UpdateUserAsync(UserForUpdateDto userForUpdate)
        {
            var currentUser = await GetCurrentUserAsync();
            var updatedUser = await _IUserRepository.GetUserAsync(currentUser.Username);

            var userEntityForUpdate = _mapper.Map<User>(userForUpdate);//لا يوجد له داعي

            if (!string.IsNullOrWhiteSpace(userEntityForUpdate.Email))
            {
                var emailNotAvailable = await _IUserRepository.EmailAvailableAsync(userEntityForUpdate.Email);
                if (emailNotAvailable)
                {
                    // return null; //return massage that email is not avalable
                }

                updatedUser.Email = userEntityForUpdate.Email.ToLower();
            }

            if (!string.IsNullOrWhiteSpace(userEntityForUpdate.Photo))
            {
                updatedUser.Photo = userEntityForUpdate.Photo;
            }
            if (!string.IsNullOrWhiteSpace(userEntityForUpdate.Bio))
            {
                updatedUser.Bio = userEntityForUpdate.Bio;
            }
            if (!string.IsNullOrWhiteSpace(userEntityForUpdate.Username))
            {
                updatedUser.Username = userEntityForUpdate.Username.ToLower();
            }

            _IUserRepository.UpdateUser(updatedUser, userEntityForUpdate);
            await _IUserRepository.SaveChangesAsync();

            //var UpdatedUserToReturn = _mapper.Map<UserDto>(updatedUser);
            //return UpdatedUserToReturn;
        }

        public async Task UpdateUserPasswordAsync(UserForUpdatePasswordDto userForUpdatePassword)
        {
            var currentUser = await GetCurrentUserAsync();
            var updatedUser = await _IUserRepository.GetUserAsync(currentUser.Username);

            if (updatedUser.Password == userForUpdatePassword.OldPassword)
            {
                updatedUser.Password = userForUpdatePassword.NewPassword;
            }
           
            _IUserRepository.UpdateUserPassword(updatedUser);
            await _IUserRepository.SaveChangesAsync();
        }

    }
}