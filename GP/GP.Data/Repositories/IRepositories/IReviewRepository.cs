using RealWord.Data.Entities;
using RealWord.Utils.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealWord.Data.Repositories
{
    public interface IReviewRepository
    {
        Task<bool> ReviewExistsAsync(Guid id); 
        Task<Review> GetReviewAsync(Guid id);
        Task<List<Review>> GetReviewsForBusinessAsync(Guid articleId);
        Task<List<Review>> GetFeedReviewsAsync(Guid currentUserId, FeedReviewsParameters feedReviewsParameters);
        Task CreateReviewAsync(Review comment);
        void DeleteReview(Review comment);
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