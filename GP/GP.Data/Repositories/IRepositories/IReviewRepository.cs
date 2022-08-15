using RealWord.Data.Entities;
using RealWord.Utils.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealWord.Data.Repositories
{
    public interface IReviewRepository
    {
        Task<bool> ReviewExistsAsync(Guid reviewId); 
        Task<Review> GetReviewAsync(Guid reviewId);
        Task<List<Review>> GetReviewsForBusinessAsync(Guid businessId);
        Task<List<Review>> GetFeedReviewsAsync(Guid currentUserId, FeedReviewsParameters feedReviewsParameters);
        Task CreateReviewAsync(Guid businessId, Guid currentUserId, Review review);
        void DeleteReview(Review review);
        Task CoolReviewAsync(Guid currentUserId, Guid reviewId);
        void UncoolReviewAsync(Guid currentUserId, Guid reviewId);
        Task<bool> IsCoolAsync(Guid currentUserId, Guid reviewId);
        Task UsefulReviewAsync(Guid currentUserId, Guid reviewId);
        void UnusfulReviewAsync(Guid currentUserId, Guid reviewId);
        Task<bool> IsUsefulAsync(Guid currentUserId, Guid reviewId);
        Task FunnyReviewAsync(Guid currentUserId, Guid reviewId);
        void UnfunnyReviewAsync(Guid currentUserId, Guid reviewId);
        Task<bool> IsFunnyAsync(Guid currentUserId, Guid reviewId);
        Task SaveChangesAsync();
    }
}