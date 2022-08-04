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
        Task<BusinessDto> LoginBusinessAsync(BusinessLoginDto BusinessLogin);
        Task<bool> BusinessExistsAsync(string businessUsername);
        Task<bool> IsAuthorized(string businessUsername);
        Task<BusinessProfileDto> GetCurrentBusinessAsync();
        Task<Guid> GetCurrentBusinessIdAsync(string businessUsername);
        Task<BusinessProfileDto> GetBusinessProfileAsync(string businessUsername);
        Task<IEnumerable<BusinessProfileDto>> GetBusinessesAsync(BusinessesParameters businessesParameters);
        Task<List<Photo>> GetPhotosForBusinessAsync(string businessUsername);
        Task<List<UserProfileDto>> GetFollowersForBusinessAsync(string businessUsername);
        Task CreateBusinessAsync(BusinessForCreationDto businessForCreation);
        Task SetupBusinessProfile(BusinessProfileSetupDto businessProfileSetup);
        Task UpdateBusinessProfileAsync(BusinessProfileForUpdateDto businessProfileForUpdate);
        Task UpdateBusinessAsync(BusinessForUpdateDto businessForUpdate);
        Task UpdateBusinessPasswordAsync(BusinessForUpdatePasswordDto businessForUpdatePassword);
        Task DeleteBusinessAsync(string businessUsername);
        Task FollowBusinessAsync(string businessUsername);
        Task UnfollowBusinessAsync(string businessUsername);
    }
}