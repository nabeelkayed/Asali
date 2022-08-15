using RealWord.Data.Entities;
using System;
using System.Collections.Generic;
using RealWord.Utils.ResourceParameters;
using System.Threading.Tasks;
using GP.Data.Entities;

namespace RealWord.Data.Repositories
{
    public interface IBusinessRepository
    {
        Task<bool> BusinessExistsAsync(string businessUsername);
        Task<bool> EmailAvailableAsync(string email);
        Task<BusinessOwner> LoginUserAsync(BusinessOwner business);
        Task<Business> GetBusinessAsync(string businessUsername);
        Task<Business> GetBusinessByIdAsync(Guid businessId);
        Task<BusinessOwner> GetBusinessOwnerAsync(string businessName);
        Task<Business> GetBusinessAsNoTrackingAsync(string businessUsername);
        Task<List<Business>> GetBusinessesAsync(BusinessesParameters businessParameters);
        Task<List<Photo>> GetPhotosForBusinessAsync(Guid businessId);
        Task<List<User>> GetFollowersForBusinessAsync(Guid businessId);
        Task CreateBusinessAsync(BusinessOwner businessOwner);
        void SetupBusinessProfile(Business updatedBusiness, Business businessEntityForUpdate);
        void UpdateBusinessProfile(Business updatedBusiness, Business businessProfileForUpdate);
        void UpdateBusiness(BusinessOwner updatedBusiness, BusinessOwner businessForUpdate);
        void UpdateBusinessPassword(BusinessOwner updatedBusiness, BusinessOwner businessEntityForUpdate);
        void DeleteBusiness(Business business);
        Task FollowBusinessAsync(Guid currentUserId, Guid businessToFollowId);
        void UnfollowBusiness(Guid currentUserId, Guid businessToUnfollowId);
        Task<bool> IsFollowedAsync(Guid UserId, Guid businessId);
        Task SaveChangesAsync();
    }
}