using Microsoft.EntityFrameworkCore;
using RealWord.Data.Entities;
using RealWord.Utils.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Data.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly GPDbContext _context;

        public ReviewRepository(GPDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ReviewExistsAsync(Guid reviewId)
        {
            if (reviewId == null || reviewId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(reviewId));
            }

            bool reviewExists = await _context.Reviews.AnyAsync(c => c.ReviewId == reviewId);
            return reviewExists;
        }
        public async Task<Review> GetReviewAsync(Guid reviewId)
        {
            if (reviewId == null || reviewId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(reviewId));
            }

            var review = await _context.Reviews.Include(r => r.User)
                                                 .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
            return review;
        }
        public async Task<List<Review>> GetReviewsForBusinessAsync(Guid businessId)
        {
            var businessReviews = await _context.Reviews.Where(c => c.BusinessId == businessId)
                                                   .Include(c => c.User)
                                                   .ThenInclude(c => c.Followings)
                                                   .ToListAsync();
            return businessReviews;
        }
        public async Task<List<Review>> GetFeedReviewsAsync(Guid currentUserId, FeedReviewsParameters feedReviewsParameters)
        {
            var userFollowings = await _context.BusinessFollowers.Where(u => u.UserId == currentUserId)
                                                                 .Select(u => u.BusinessId)
                                                                 .ToListAsync();
            if (!userFollowings.Any())
            {
                throw new ArgumentNullException(nameof(userFollowings));
            }

            var feedReviews = await _context.Reviews.Where(r => userFollowings.Contains(r.BusinessId))
                                                .Include(r => r.User)
                                                .ThenInclude(r => r.Followings)
                                                .OrderByDescending(r => r.CreatedAt)
                                                .Skip(feedReviewsParameters.Offset)
                                                .Take(feedReviewsParameters.Limit)
                                                .ToListAsync();
            return feedReviews;
        }
        public async Task CreateReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }
        public void DeleteReview(Review review)
        {
            _context.Reviews.Remove(review);
        }

        public async Task CoolReviewAsync(Guid currentUserId, Guid reviewId)
        {
            var CoolReview =
                new ReviewCool { ReviewId = reviewId, UserId = currentUserId };
            await _context.ReviewCool.AddAsync(CoolReview);
        }

        public void UncoolReviewAsync(Guid currentUserId, Guid reviewId)
        {
            var CoolReview =
                new ReviewCool { ReviewId = reviewId, UserId = currentUserId };
            _context.ReviewCool.Remove(CoolReview);
        }

        public async Task<bool> IsCoolAsync(Guid currentUserId, Guid reviewId)
        {
            var isCool =
                         await _context.ReviewCool.AnyAsync(af => af.UserId == currentUserId && af.ReviewId == reviewId);
            return isCool;
        }

        public async Task UsefulReviewAsync(Guid currentUserId, Guid reviewId)
        {
            var usfulReview =
                new ReviewUseful { ReviewId = reviewId, UserId = currentUserId };
            await _context.ReviewUseful.AddAsync(usfulReview);
        }

        public void UnusfulReviewAsync(Guid currentUserId, Guid reviewId)
        {
            var usfulReview =
                new ReviewUseful { ReviewId = reviewId, UserId = currentUserId };
            _context.ReviewUseful.Remove(usfulReview);
        }

        public async Task<bool> IsUsefulAsync(Guid currentUserId, Guid reviewId)
        {
            var isUsful =
                await _context.ReviewCool.AnyAsync(af => af.UserId == currentUserId && af.ReviewId == reviewId);
            return isUsful;
        }

        public async Task FunnyReviewAsync(Guid currentUserId, Guid reviewId)
        {
            var funnyReview =
                new ReviewFunny { ReviewId = reviewId, UserId = currentUserId };
            await _context.ReviewFunny.AddAsync(funnyReview);
        }

        public void UnfunnyReviewAsync(Guid currentUserId, Guid reviewId)
        {
            var funnyReview =
                new ReviewFunny { ReviewId = reviewId, UserId = currentUserId };
            _context.ReviewFunny.Remove(funnyReview);
        }

        public async Task<bool> IsFunnyAsync(Guid currentUserId, Guid reviewId)
        {
            var isFunny =
                await _context.ReviewCool.AnyAsync(af => af.UserId == currentUserId && af.ReviewId == reviewId);
            return isFunny;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
