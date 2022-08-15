using System;
using System.Collections.Generic;
using RealWord.Utils.ResourceParameters;
using System.Threading.Tasks;
using RealWord.Core.Models;
using GP.Core.Models;
using RealWord.Data.Entities;

namespace GP.Core.Services
{
    public interface IBusinessService
    {
        Task<bool> BusinessExistsAsync(string businessUsername);
        Task<bool> IsAuthorized(string businessUsername);
        Task<BusinessDto> LoginBusinessAsync(BusinessLoginDto BusinessLogin);
        Task<BusinessProfileDto> GetCurrentBusinessAsync();
        Task<Guid> GetCurrentBusinessIdAsync(string businessUsername);
        Task<BusinessProfileDto> GetBusinessProfileAsync(string businessUsername);
        Task<IEnumerable<BusinessProfileDto>> GetBusinessesAsync(BusinessesParameters businessesParameters);
        Task<List<Photo>> GetPhotosForBusinessAsync(Guid businessId);
        Task<List<UserProfileDto>> GetFollowersForBusinessAsync(Guid businessId);
        Task<bool> CreateBusinessAsync(BusinessForCreationDto businessForCreation);
        Task<bool> SetupBusinessProfileAsync(BusinessProfileSetupDto businessProfileSetup);
        Task<bool> UpdateBusinessProfileAsync(BusinessProfileForUpdateDto businessProfileForUpdate);
        Task<bool> UpdateBusinessAsync(BusinessForUpdateDto businessForUpdate);
        Task<bool> UpdateBusinessPasswordAsync(BusinessForUpdatePasswordDto businessForUpdatePassword);
        Task<bool> DeleteBusinessAsync(string businessUsername);
        Task<bool> FollowBusinessAsync(string businessUsername);
        Task<bool> UnfollowBusinessAsync(string businessUsername);
    }
}