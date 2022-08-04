using RealWord.Core.Models;
using RealWord.Utils.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealWord.Core.Services
{
    public interface IReviewService
    {
        Task<bool> ReviewExistsAsync(string businessUsername, Guid reviewId);
        Task<bool> IsAuthorized(string businessUsername, Guid reviewId);
        Task<ReviewDto> GetReviewAsync(Guid reviewId);
        Task<IEnumerable<ReviewDto>> GetReviewsForBusinessAsync(string businessUsername);
        Task<IEnumerable<ReviewDto>> GetFeedReviewsAsync(FeedReviewsParameters feedReviewsParameters);
        Task CreateReviewForBusinessAsync(string businessUsername, ReviewForCreationDto reviewForCreation);
        Task DeleteReviewAsync(Guid reviewId);
        Task CoolReviewAsync(Guid reviewId); 
        Task UncoolReviewAsync(Guid reviewId);
        Task UsefulReviewAsync(Guid reviewId);
        Task UnusefulReviewAsync(Guid reviewId);
        Task FunnyReviewAsync(Guid reviewId);
        Task UnfunnyReviewAsync(Guid reviewId);
    }
}