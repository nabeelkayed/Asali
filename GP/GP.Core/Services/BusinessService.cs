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

        public async Task<bool> BusinessExistsAsync(Guid businessId)
        {
            var businessExists = await _IBusinessRepository.BusinessExistsAsync(businessId);
            return businessExists;
        }

        public async Task<bool> IsAuthorized(Guid businessId)
        {
            var business = await _IBusinessRepository.GetBusinessByIdAsync(businessId);
            var currentBusiness = await GetCurrentBusinessAsync();

            var isAuthorized = currentBusiness.BusinessId == business.BusinessId;
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
            /*var business1 = await _IBusinessRepository.GetBusinessByIdAsync(businesslogedin.BusinessId);
            businessToReturn.BusinessName = business1.BusinessName;*/
            businessToReturn.Token = _IBusinessAuth.Generate(businesslogedin);

            return businessToReturn;
        }

        public async Task<BusinessProfileDto> GetCurrentBusinessAsync()
        {
            var currentBusinessOwnerEmail = _accessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (!String.IsNullOrEmpty(currentBusinessOwnerEmail))
            {
                var currentBusiness = await _IBusinessRepository.GetBusinessAsNoTrackingAsync(currentBusinessOwnerEmail);
                var businessToReturn = _mapper.Map<BusinessProfileDto>(currentBusiness);
                return businessToReturn;
            }

            return null;
        }

      /*  public async Task<Guid> GetCurrentBusinessIdAsync(string businessUsername) //see the user function
        {
            var business = await _IBusinessRepository.GetBusinessAsync(businessUsername);

            var businessId = business?.BusinessId ?? Guid.Empty;
            return businessId;
        }*/

        public async Task<BusinessProfileDto> GetBusinessProfileAsync(Guid businessId)
        {
            var business = await _IBusinessRepository.GetBusinessByIdAsync(businessId);
            if (business == null)
            {
                return null;
            }

            var businessToReturn = MapBusiness(business, Guid.Empty);//map business to business profile
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

        public async Task<List<Photo>> GetPhotosForBusinessAsync(Guid businessId)
        {
           /* var isExists = await _IBusinessRepository.BusinessExistsAsync(businessId);
            if (!isExists)
            {
                return null;
            }
           */

            var photos = await _IBusinessRepository.GetPhotosForBusinessAsync(businessId);
            //we should add maping
            return photos;
        }

        public async Task<List<UserProfileDto>> GetFollowersForBusinessAsync(Guid businessId)
        {
           /* var isExist = await _IBusinessRepository.BusinessExistsAsync(businessUsername);
            if (!isExist)
            {
                return null;
            }*/

            var followers = await _IBusinessRepository.GetFollowersForBusinessAsync(businessId);

            var followersToReturn = _mapper.Map<List<UserProfileDto>>(followers);

           // var followersToReturn = new List<UserProfileDto>();
            //var currentUserId = await _IUserService.GetCurrentUserIdAsync();

           /* foreach (var follower in followers)
            {
                var userDto = MapUser(follower, currentUserId);


                var businessToReturn = _mapper.Map<BusinessProfileDto>(business, b => b.Items["currentUserId"] = currentUserId);
                // var profileDto = _mapper.Map<BusinessProfileDto>(business.User, b => b.Items["currentUserId"] = currentUserId);
                // articleToReturn.Author = profileDto;


                followersToReturn.Add(userDto);
            }*/

            return followersToReturn;
        }

        public async Task<bool> CreateBusinessAsync(BusinessForCreationDto businessForCreation)
        {
            businessForCreation.Email = businessForCreation.Email.ToLower();
            businessForCreation.Password = businessForCreation.Password.GetHash();

            var emailNotAvailable = await _IBusinessRepository.EmailAvailableAsync(businessForCreation.Email);
            if (emailNotAvailable)
            {
                return false;
            }

            var businessOwnerEntityForCreation = _mapper.Map<BusinessOwner>(businessForCreation);

            await _IBusinessRepository.CreateBusinessAsync(businessOwnerEntityForCreation);
            await _IBusinessRepository.SaveChangesAsync();

            return true;

            /* if (businessForCreation.TagList != null && businessForCreation.TagList.Any())
             {
                 await _ITagService.CreateTags(businessForCreation.TagList, businessEntityForCreation.BusinessId);
             }*/
        }

        public async Task<bool> SetupBusinessProfileAsync(BusinessProfileSetupDto businessProfileSetup)
        {
            var currentBusiness = await GetCurrentBusinessAsync();
            var updatedBusiness = await _IBusinessRepository.GetBusinessByIdAsync(currentBusiness.BusinessId);

            var businessEntityForUpdate = _mapper.Map<Business>(businessProfileSetup);

            _IBusinessRepository.SetupBusinessProfile(updatedBusiness, businessEntityForUpdate);

            await _IBusinessRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateBusinessProfileAsync(BusinessProfileForUpdateDto businessProfileForUpdate)
        {
            var currentBusiness = await GetCurrentBusinessAsync();
            var updatedBusiness = await _IBusinessRepository.GetBusinessAsync(currentBusiness.BusinessId);

            var businessEntityForUpdate = _mapper.Map<Business>(businessProfileForUpdate);

            _IBusinessRepository.UpdateBusinessProfile(updatedBusiness, businessEntityForUpdate);
            await _IBusinessRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateBusinessAsync(BusinessForUpdateDto businessForUpdate)
        {
            var currentBusiness = await GetCurrentBusinessAsync();
            var updatedBusiness = await _IBusinessRepository.GetBusinessOwnerAsync(currentBusiness.BusinessName);

            var businessEntityForUpdate = _mapper.Map<BusinessOwner>(businessForUpdate);

            _IBusinessRepository.UpdateBusiness(updatedBusiness, businessEntityForUpdate);
            await _IBusinessRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateBusinessPasswordAsync(BusinessForUpdatePasswordDto businessForUpdatePassword)
        {
            var currentBusiness = await GetCurrentBusinessAsync();
            var updatedBusiness = await _IBusinessRepository.GetBusinessOwnerAsync(currentBusiness.BusinessName);

            var businessOwnerEntityForUpdate = _mapper.Map<BusinessOwner>(businessForUpdatePassword);
            businessForUpdatePassword.OldPassword= businessForUpdatePassword.OldPassword.GetHash();
            if (updatedBusiness.Password == businessForUpdatePassword.OldPassword)
            {
                updatedBusiness.Password = businessForUpdatePassword.NewPassword.GetHash();

                _IBusinessRepository.UpdateBusinessPassword(updatedBusiness, businessOwnerEntityForUpdate);
                await _IBusinessRepository.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteBusinessAsync(Guid businessId)
        {
            var business = await _IBusinessRepository.GetBusinessAsync(businessId);

            _IBusinessRepository.DeleteBusiness(business);
            await _IBusinessRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> FollowBusinessAsync(Guid businessId)
        {
            var business = await _IBusinessRepository.GetBusinessByIdAsync(businessId);
            if (business == null)
            {
                return false;
            }

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();
            var isFavorited = await _IBusinessRepository.IsFollowedAsync(currentUserId, business.BusinessId);
            if (isFavorited)
            {
                return false;
            }

            await _IBusinessRepository.FollowBusinessAsync(currentUserId, business.BusinessId);
            await _IBusinessRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnfollowBusinessAsync(Guid businessId)
        {
            var business = await _IBusinessRepository.GetBusinessAsync(businessId);
            if (business == null)
            {
                return false;
            }

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();
            var isFavorited = await _IBusinessRepository.IsFollowedAsync(currentUserId, business.BusinessId);
            if (!isFavorited)
            {
                return false;
            }

            _IBusinessRepository.UnfollowBusiness(currentUserId, business.BusinessId);
            await _IBusinessRepository.SaveChangesAsync();

            return true;
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
