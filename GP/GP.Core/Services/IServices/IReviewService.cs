using RealWord.Core.Models;
using RealWord.Utils.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealWord.Core.Services
{
    public interface IReviewService
    {
        Task<bool> ReviewExistsAsync(Guid businessId, Guid reviewId);
        Task<bool> IsAuthorized(Guid businessId, Guid reviewId);
        Task<ReviewDto> GetReviewAsync(Guid reviewId);
        Task<IEnumerable<ReviewDto>> GetReviewsForBusinessAsync(Guid businessId);
        Task<IEnumerable<ReviewDto>> GetFeedReviewsAsync(FeedReviewsParameters feedReviewsParameters);
        Task<bool> CreateReviewForBusinessAsync(Guid businessId, ReviewForCreationDto reviewForCreation);
        Task<bool> DeleteReviewAsync(Guid reviewId);
        Task<bool> CoolReviewAsync(Guid reviewId); 
        Task<bool> UncoolReviewAsync(Guid reviewId);
        Task<bool> UsefulReviewAsync(Guid reviewId);
        Task<bool> UnusefulReviewAsync(Guid reviewId);
        Task<bool> FunnyReviewAsync(Guid reviewId);
        Task<bool> UnfunnyReviewAsync(Guid reviewId);
    }
}