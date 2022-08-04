using RealWord.Data.Entities;
using RealWord.Utils.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using RealWord.Utils.Utils;
using System.Threading.Tasks;
using RealWord.Core.Models;
using RealWord.Data.Repositories;
using AutoMapper;
using RealWord.Core.Services;
using GP.Core.Models;
using RealWord.Core.Auth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using GP.Data.Entities;

namespace GP.Core.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _IBusinessRepository;
        private readonly IUserService _IUserService;
        private readonly ITagService _ITagService;
        private readonly IBusinessAuth _IBusinessAuth;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public BusinessService(IBusinessRepository businessRepository, IUserService userService,
         ITagService tagService, IBusinessAuth businessAuth, IHttpContextAccessor accessor, IMapper mapper)
        {

            _IBusinessRepository = businessRepository ??
                throw new ArgumentNullException(nameof(businessRepository));
            _IUserService = userService ??
                throw new ArgumentNullException(nameof(userService));
            _ITagService = tagService ??
                throw new ArgumentNullException(nameof(tagService));
            _IBusinessAuth = businessAuth ??
                throw new ArgumentNullException(nameof(businessAuth));
            _accessor = accessor ??
                throw new ArgumentNullException(nameof(accessor));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> BusinessExistsAsync(string businessUsername)
        {
            var businessExists = await _IBusinessRepository.BusinessExistsAsync(businessUsername);
            return businessExists;
        }

        public async Task<bool> IsAuthorized(string businessUsername)
        {
            var business = await _IBusinessRepository.GetBusinessAsync(businessUsername);
            var currentBusinessId = await GetCurrentBusinessIdAsync(businessUsername);

            var isAuthorized = currentBusinessId == business.BusinessId;
            return isAuthorized;
        }

        public async Task<BusinessDto> LoginBusinessAsync(BusinessLoginDto businessLogin)
        {
            businessLogin.Email = businessLogin.Email.ToLower();
            businessLogin.Password = businessLogin.Password.GetHash();

            var business = _mapper.Map<BusinessOwner>(businessLogin);
            var businesslogedin = await _IBusinessRepository.LoginUserAsync(business);
            if (businesslogedin == null)
            {
                return null;
            }

            var businessToReturn = _mapper.Map<BusinessDto>(businesslogedin);
            businessToReturn.Token = _IBusinessAuth.Generate(businesslogedin);
            return businessToReturn;
        }

        public async Task<BusinessProfileDto> GetCurrentBusinessAsync()
        {
            var currentBusinessUsername = _accessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!String.IsNullOrEmpty(currentBusinessUsername))
            {
                var currentBusiness = await _IBusinessRepository.GetBusinessAsNoTrackingAsync(currentBusinessUsername);
                var businessToReturn = _mapper.Map<BusinessProfileDto>(currentBusiness);
                return businessToReturn;
            }

            return null;
        }

        public async Task<Guid> GetCurrentBusinessIdAsync(string businessUsername)
        {
            var business = await _IBusinessRepository.GetBusinessAsync(businessUsername);

            var businessId = business?.BusinessId ?? Guid.Empty;
            return businessId;
        }

        public async Task<BusinessProfileDto> GetBusinessProfileAsync(string businessUsername)
        {
            var business = await _IBusinessRepository.GetBusinessAsync(businessUsername);
            if (business == null)
            {
                return null;
            }

            var businessToReturn = MapBusiness(business, Guid.Empty);
            return businessToReturn;
        }

        public async Task<IEnumerable<BusinessProfileDto>> GetBusinessesAsync(BusinessesParameters businessesParameters)
        {
            var businesses = await _IBusinessRepository.GetBusinessesAsync(businessesParameters);
            if (businesses == null)
            {
                return null;
            }

            var businessesToReturn = new List<BusinessProfileDto>();
            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            foreach (var business in businesses)
            {
                var businessDto = MapBusiness(business, currentUserId);
                businessesToReturn.Add(businessDto);
            }

            return businessesToReturn;
        }

        public async Task<List<Photo>> GetPhotosForBusinessAsync(string businessUsername)
        {
            var isExists = await _IBusinessRepository.BusinessExistsAsync(businessUsername);
            if (!isExists)
            {
                return null;
            }

            var photos = await _IBusinessRepository.GetPhotosForBusinessAsync(businessUsername);
            //we should add maping
            return photos;
        }

        public async Task<List<UserProfileDto>> GetFollowersForBusinessAsync(string businessUsername)
        {
            var isExist = await _IBusinessRepository.BusinessExistsAsync(businessUsername);
            if (!isExist)
            {
                return null;
            }
            var followers = await _IBusinessRepository.GetFollowersForBusinessAsync(businessUsername);

            var followersToReturn = new List<UserProfileDto>();
            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            foreach (var follower in followers)
            {
                var userDto = MapUser(follower, currentUserId);
                followersToReturn.Add(userDto);
            }

            return followersToReturn;
        }

        public async Task CreateBusinessAsync(BusinessForCreationDto businessForCreation)
        {
            var businessOwnerEntityForCreation = _mapper.Map<BusinessOwner>(businessForCreation);

            businessOwnerEntityForCreation.BusinessOwnerId = Guid.NewGuid();

            //var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            await _IBusinessRepository.CreateBusinessAsync(businessOwnerEntityForCreation);
            await _IBusinessRepository.SaveChangesAsync();

           /* if (businessForCreation.TagList != null && businessForCreation.TagList.Any())
            {
                await _ITagService.CreateTags(businessForCreation.TagList, businessEntityForCreation.BusinessId);
            }*/

           // var createdBusiness = await _IBusinessRepository.GetBusinessAsync(businessEntityForCreation.BusinessUsername);
           // var createdBusinessToReturn = MapBusiness(createdBusiness, currentUserId);
            //return createdBusinessToReturn;
        }

        public async Task SetupBusinessProfile(BusinessProfileSetupDto businessProfileSetup)
        {
            var currentBusiness = await GetCurrentBusinessAsync();
            var updatedBusiness = await _IBusinessRepository.GetBusinessAsync(currentBusiness.BusinessName);

            var businessEntityForUpdate = _mapper.Map<Business>(businessProfileSetup);

            if (!string.IsNullOrWhiteSpace(businessEntityForUpdate.Bio))
            {
                updatedBusiness.Bio = businessEntityForUpdate.Bio;
            }
            if (!string.IsNullOrWhiteSpace(businessEntityForUpdate.Location))
            {
                updatedBusiness.Location = businessEntityForUpdate.Location;
            }

            _IBusinessRepository.UpdateBusiness(updatedBusiness, businessEntityForUpdate);
            await _IBusinessRepository.SaveChangesAsync();

           // var UpdatedBusinessToReturn = MapBusiness(updatedBusiness, /*currentUserId*/Guid.Empty);
            //return UpdatedBusinessToReturn;
        }

        public async Task UpdateBusinessProfileAsync(BusinessProfileForUpdateDto businessProfileForUpdate)
        {
            var currentBusiness = await GetCurrentBusinessAsync();

            var updatedBusiness = await _IBusinessRepository.GetBusinessAsync(currentBusiness.BusinessName);
          //  var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            var businessEntityForUpdate = _mapper.Map<Business>(businessProfileForUpdate);

            if (!string.IsNullOrWhiteSpace(businessProfileForUpdate.Bio))
            {
                updatedBusiness.Bio = businessProfileForUpdate.Bio;
            }
            if (!string.IsNullOrWhiteSpace(businessProfileForUpdate.Location))
            {
                updatedBusiness.Location = businessProfileForUpdate.Location;
            }

            _IBusinessRepository.UpdateBusiness(updatedBusiness, businessEntityForUpdate);
            await _IBusinessRepository.SaveChangesAsync();

            //var UpdatedBusinessToReturn = MapBusiness(updatedBusiness, currentUserId);
            //return UpdatedBusinessToReturn;
        }
        public async Task UpdateBusinessAsync(BusinessForUpdateDto businessForUpdate)
        {
            var currentBusiness = await GetCurrentBusinessAsync();

            var updatedBusiness = await _IBusinessRepository.GetBusinessAsync(currentBusiness.BusinessName);
           // var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            var businessEntityForUpdate = _mapper.Map<Business>(businessForUpdate);

            if (!string.IsNullOrWhiteSpace(businessEntityForUpdate.Bio))
            {
                updatedBusiness.Bio = businessEntityForUpdate.Bio;
            }
            if (!string.IsNullOrWhiteSpace(businessEntityForUpdate.Location))
            {
                updatedBusiness.Location = businessEntityForUpdate.Location;
            }

            _IBusinessRepository.UpdateBusiness(updatedBusiness, businessEntityForUpdate);
            await _IBusinessRepository.SaveChangesAsync();

           // var UpdatedBusinessToReturn = MapBusiness(updatedBusiness, currentUserId);
            //return UpdatedBusinessToReturn;
        }
        public async Task UpdateBusinessPasswordAsync(BusinessForUpdatePasswordDto businessForUpdatePassword)
        {
            var currentBusiness = await GetCurrentBusinessAsync();

            var updatedBusiness = await _IBusinessRepository.GetBusinessAsync(currentBusiness.BusinessName);
            //var updatedBusiness1 = await _IBusinessRepository.GetBusinessOwnerAsync(currentBusiness.BusinessName);

            var businessEntityForUpdate = _mapper.Map<Business>(businessForUpdatePassword);

           /* if (updatedBusiness.Password == businessForUpdatePassword.OldPassword)
            {
                updatedBusiness.Password = businessForUpdatePassword.NewPassword;
            }*///مهم

            //return null;
            
            _IBusinessRepository.UpdateBusinessPassword(updatedBusiness, businessEntityForUpdate);
            await _IBusinessRepository.SaveChangesAsync();
        }

        public async Task DeleteBusinessAsync(string businessUsername)
        {
            var business = await _IBusinessRepository.GetBusinessAsync(businessUsername);

            _IBusinessRepository.DeleteBusiness(business);
            await _IBusinessRepository.SaveChangesAsync();
        }

        public async Task FollowBusinessAsync(string businessUsername)
        {
            var business = await _IBusinessRepository.GetBusinessAsync(businessUsername);
            if (business == null)
            {
                //return null;
            }

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            var isFavorited = await _IBusinessRepository.IsFollowedAsync(currentUserId, business.BusinessId);
            if (isFavorited)
            {
                //return null;
            }

            await _IBusinessRepository.FollowBusinessAsync(currentUserId, business.BusinessId);
            await _IBusinessRepository.SaveChangesAsync();

            //var followBusinessToReturn = MapBusiness(business, currentUserId);
            //return followBusinessToReturn;
        }
        public async Task UnfollowBusinessAsync(string businessUsername)
        {
            var business = await _IBusinessRepository.GetBusinessAsync(businessUsername);
            if (business == null)
            {
                //return null;
            }

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            var isFavorited = await _IBusinessRepository.IsFollowedAsync(currentUserId, business.BusinessId);
            if (!isFavorited)
            {
                //return null;
            }

            _IBusinessRepository.UnfollowBusiness(currentUserId, business.BusinessId);
            await _IBusinessRepository.SaveChangesAsync();

            //var unfollowedBusinessToReturn = MapBusiness(business, currentUserId);
            //return unfollowedBusinessToReturn;
        }
        private BusinessProfileDto MapBusiness(Business business, Guid currentUserId)
        {
            var businessToReturn = _mapper.Map<BusinessProfileDto>(business, b => b.Items["currentUserId"] = currentUserId);
           // var profileDto = _mapper.Map<BusinessProfileDto>(business.User, b => b.Items["currentUserId"] = currentUserId);
           // articleToReturn.Author = profileDto;

            return businessToReturn;
        }
        private UserProfileDto MapUser(User user, Guid currentUserId)
        {
            var businessToReturn = _mapper.Map<UserProfileDto>(user, b => b.Items["currentUserId"] = currentUserId);
            // var profileDto = _mapper.Map<BusinessProfileDto>(business.User, b => b.Items["currentUserId"] = currentUserId);
            // articleToReturn.Author = profileDto;

            return businessToReturn;
        }
    }
}
