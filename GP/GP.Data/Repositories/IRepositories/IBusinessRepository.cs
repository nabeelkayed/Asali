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
        Task<BusinessOwner> LoginUserAsync(BusinessOwner business);
        Task<Business> GetBusinessAsync(string businessUsername);
        Task<Business> GetBusinessAsNoTrackingAsync(string businessUsername);
        Task<List<Business>> GetBusinessesAsync(BusinessesParameters businessParameters);
        Task<List<Photo>> GetPhotosForBusinessAsync(string businessUsername);
        Task<List<User>> GetFollowersForBusinessAsync(string businessUsername);
        Task CreateBusinessAsync(BusinessOwner businessOwner);
        void UpdateBusiness(Business updatedBusiness, Business businessForUpdate);
        void UpdateBusinessPassword(Business updatedBusiness, Business businessEntityForUpdate);
        void DeleteBusiness(Business business);
        Task FollowBusinessAsync(Guid currentUserId, Guid businessToFollowId);
        void UnfollowBusiness(Guid currentUserId, Guid businessToUnfollowId);
        Task<bool> IsFollowedAsync(Guid UserId, Guid businessId);
        Task SaveChangesAsync();
    }
}