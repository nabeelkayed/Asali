using GP.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealWord.Data.Entities;
using RealWord.Utils.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Data.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly GPDbContext _context;

        public BusinessRepository(GPDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> BusinessExistsAsync(string businessUsername)
        {
            if (String.IsNullOrEmpty(businessUsername))
            {
                throw new ArgumentNullException(nameof(businessUsername));
            }

            bool businessExists = await _context.Businesses.AnyAsync(b => b.BusinessUsername == businessUsername);
            return businessExists;
        }

        public async Task<BusinessOwner> LoginUserAsync(BusinessOwner businessOwner)
        {
            var LoginBusiness = await _context.BusinessOwners.FirstOrDefaultAsync(b => b.Email == businessOwner.Email
                                                                       && b.Password == businessOwner.Password);
            return LoginBusiness;
        }

        public async Task<Business> GetBusinessAsync(string businessUsername)
        {
            if (String.IsNullOrEmpty(businessUsername))
            {
                throw new ArgumentNullException(nameof(businessUsername));
            }

            var business = await _context.Businesses.Include(b => b.Tags)
                                                 .FirstOrDefaultAsync(b => b.BusinessUsername == businessUsername);
            return business;
        }

        public async Task<Business> GetBusinessAsNoTrackingAsync(string businessUsername)
        {
            if (String.IsNullOrEmpty(businessUsername))
            {
                throw new ArgumentNullException(nameof(businessUsername));
            }

            var business = await _context.Businesses.AsNoTracking()
                                           .Include(b => b.Reviews)
                                           .FirstOrDefaultAsync(b => b.BusinessName == businessUsername);
            return business;
        }

        public async Task<List<Business>> GetBusinessesAsync(BusinessesParameters businessesParameters)
        {
            var businesses = _context.Businesses.AsQueryable();

           /* if (!string.IsNullOrEmpty(businessesParameters.Tag))
            {
                var tag = businessesParameters.Tag.Trim();
                var userfavarets = await _context.ArticleTags.Where(af => af.TagId == tag)
                                                       .Select(a => a.ArticleId)
                                                       .ToListAsync();
                businesses = businesses.Where(a => userfavarets.Contains(a.BusinessId));
            }*/
            if (!string.IsNullOrEmpty(businessesParameters.Category))
            {
                //var authorname = businessesParameters.Category.Trim();
                //var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == authorname);
                businesses = businesses.Where(b => b.Category == businessesParameters.Category);
            }
            if (!string.IsNullOrEmpty(businessesParameters.Followed))
            {
                var username = businessesParameters.Followed.Trim();
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

                var userfavarets = await _context.BusinessFollowers.Where(bf => bf.UserId == user.UserId)
                                                            .Select(b => b.BusinessId)
                                                            .ToListAsync();
                businesses = businesses.Where(b => userfavarets.Contains(b.BusinessId));
            }

            businesses = businesses.Skip(businessesParameters.Offset)
                               .Take(businessesParameters.Limit)
                             /*  .Include(a => a.User)
                               .ThenInclude(a => a.Followers)
                               .Include(a => a.Tags)
                               .Include(a => a.Favorites)
                               .OrderByDescending(x => x.CreatedAt)*/;

            var allBusinesses = await businesses.ToListAsync();
            return allBusinesses;
        }

        public async Task<List<Photo>> GetPhotosForBusinessAsync(string businessUsername)
        {
            var business = await _context.Businesses.FirstOrDefaultAsync(b => b.BusinessName == businessUsername);

            var photos = await _context.Photos.Where(b => b.BusinessId == business.BusinessId).ToListAsync();
            return photos;
        }

        public async Task<List<User>> GetFollowersForBusinessAsync(string businessUsername)
        {
            var business = await _context.Businesses.FirstOrDefaultAsync(b => b.BusinessName == businessUsername);

            var followers = await _context.BusinessFollowers.Where(b => b.BusinessId == business.BusinessId).Select(a => a.User).ToListAsync();
            return followers;
        }

        public async Task CreateBusinessAsync(BusinessOwner businessOwner)
        {
            var business = new Business { BusinessId = Guid.NewGuid() };
            await _context.Businesses.AddAsync(business);

            businessOwner.BusinessId = business.BusinessId;
            await _context.BusinessOwners.AddAsync(businessOwner);
        }

        public void UpdateBusiness(Business updatedBusiness, Business businessForUpdate)
        {

        }

        public void UpdateBusinessPassword(Business updatedBusiness, Business businessEntityForUpdate)
        {

        }

        public void DeleteBusiness(Business business)
        {
            _context.Businesses.Remove(business);
        }

        public async Task FollowBusinessAsync(Guid currentUserId, Guid businessToFollowId)
        {
            var businessFollower =
                new BusinessFollowers { BusinessId = businessToFollowId, UserId = currentUserId };
            await _context.BusinessFollowers.AddAsync(businessFollower);
        }

        public void UnfollowBusiness(Guid currentUserId, Guid businessToUnfollowId)
        {
            var businessFollower =
                new BusinessFollowers { BusinessId = businessToUnfollowId, UserId = currentUserId };
            _context.BusinessFollowers.Remove(businessFollower);
        }

        public async Task<bool> IsFollowedAsync(Guid UserId, Guid businessId)
        {
            var isFavorited =
               await _context.BusinessFollowers.AnyAsync(bf => bf.UserId == UserId && bf.BusinessId == businessId);
            return isFavorited;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
