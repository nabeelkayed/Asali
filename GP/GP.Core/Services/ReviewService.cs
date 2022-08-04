using AutoMapper;
using GP.Core.Models;
using GP.Core.Services;
using RealWord.Core.Models;
using RealWord.Data.Entities;
using RealWord.Data.Repositories;
using RealWord.Utils.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _IReviewRepository;
        private readonly IBusinessService _IBusinessService;
        private readonly IUserService _IUserService;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository,
        IBusinessService businessService, IUserService userService, IMapper mapper)
        {
            _IReviewRepository = reviewRepository ??
               throw new ArgumentNullException(nameof(reviewRepository));
            _IBusinessService = businessService ??
               throw new ArgumentNullException(nameof(businessService));
            _IUserService = userService ??
                throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> ReviewExistsAsync(string businessUsername, Guid reviewId)
        {
            var businessExists = await _IBusinessService.BusinessExistsAsync(businessUsername);
            if (!businessExists)
            {
                return false;
            }

            var reviewExists = await _IReviewRepository.ReviewExistsAsync(reviewId);
            if (!reviewExists)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsAuthorized(string businessUsername, Guid reviewId)
        {
            var review = await _IReviewRepository.GetReviewAsync(reviewId);
            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            var isAuthorized = currentUserId == review.UserId;
            return isAuthorized;
        }

        public async Task<ReviewDto> GetReviewAsync(Guid reviewId)
        {
            var review = await _IReviewRepository.GetReviewAsync(reviewId);
            if (review == null)
            {
                return null;
            }

            var reviewToReturn = MapReview(Guid.Empty, review);
            return reviewToReturn;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsForBusinessAsync(string businessUsername)
        {
            var businessId = await _IBusinessService.GetCurrentBusinessIdAsync(businessUsername);
            if (businessId == Guid.Empty)
            {
                return null;
            }

            var reviews = await _IReviewRepository.GetReviewsForBusinessAsync(businessId);

            var reviewsToReturn = new List<ReviewDto>();
            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            foreach (var review in reviews)
            {
                var reviewsDto = MapReview(currentUserId, review);
                reviewsToReturn.Add(reviewsDto);
            }

            return reviewsToReturn;
        }

        public async Task<IEnumerable<ReviewDto>> GetFeedReviewsAsync(FeedReviewsParameters feedReviewsParameters)
        {
            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            var reviews = await _IReviewRepository.GetFeedReviewsAsync(currentUserId, feedReviewsParameters);
            if (!reviews.Any())
            {
                return null;
            }

            var reviewsToReturn = new List<ReviewDto>();
            foreach (var review in reviews)
            {
                var reviewDto = MapReview(currentUserId, review );
                reviewsToReturn.Add(reviewDto);
            }

            return reviewsToReturn;
        }

        public async Task CreateReviewForBusinessAsync(string businessUsername, ReviewForCreationDto reviewForCreationDto)
        {
            var businessId = await _IBusinessService.GetCurrentBusinessIdAsync(businessUsername);
            if (businessId == Guid.Empty)
            {
               // return null;
            }

            var reviewEntityForCreation = _mapper.Map<Review>(reviewForCreationDto);

            reviewEntityForCreation.ReviewId = Guid.NewGuid();

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();
            reviewEntityForCreation.UserId = currentUserId;

            reviewEntityForCreation.BusinessId = businessId;

            var timeStamp = DateTime.Now;
            reviewEntityForCreation.CreatedAt = timeStamp;

            await _IReviewRepository.CreateReviewAsync(reviewEntityForCreation);
            await _IReviewRepository.SaveChangesAsync();

            //var createdReviewToreturn = MapReview(currentUserId, reviewEntityForCreation);
            //return createdReviewToreturn;
        }

        public async Task DeleteReviewAsync(Guid reviewId)
        {
            var review = await _IReviewRepository.GetReviewAsync(reviewId);

            _IReviewRepository.DeleteReview(review);
            await _IReviewRepository.SaveChangesAsync();
        }
        public async Task CoolReviewAsync(Guid reviewId)
        {
            var review = await _IReviewRepository.GetReviewAsync(reviewId);
            if (review == null)
            {
               // return null;
            }

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            var isCool = await _IReviewRepository.IsCoolAsync(currentUserId, review.ReviewId);
            if (isCool)
            {
                //return null;
            }

            await _IReviewRepository.CoolReviewAsync(currentUserId, review.ReviewId);
            await _IReviewRepository.SaveChangesAsync();

          //  var coolReviewToReturn = MapReview(currentUserId, review);
         //   return coolReviewToReturn;
        }

        public async Task UncoolReviewAsync(Guid reviewId)
        {
            var review = await _IReviewRepository.GetReviewAsync(reviewId);
            if (review == null)
            {
               // return null;
            }

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            var isCool = await _IReviewRepository.IsCoolAsync(currentUserId, review.ReviewId);
            if (isCool)
            {
                //return null;
            }

            _IReviewRepository.UncoolReviewAsync(currentUserId, review.ReviewId);
            await _IReviewRepository.SaveChangesAsync();

       //     var uncoolReviewToReturn = MapReview(currentUserId, review);
         //   return uncoolReviewToReturn;
        }

        public async Task UsefulReviewAsync(Guid reviewId)
        {
            var review = await _IReviewRepository.GetReviewAsync(reviewId);
            if (review == null)
            { 
               // return null;
            }

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            var isUseful = await _IReviewRepository.IsUsefulAsync(currentUserId, review.ReviewId);
            if (isUseful)
            {
               // return null;
            }

            await _IReviewRepository.UsefulReviewAsync(currentUserId, review.ReviewId);
            await _IReviewRepository.SaveChangesAsync();

            //var usefulReviewToReturn = MapReview(currentUserId, review);
            //return usefulReviewToReturn;
        }

        public async Task UnusefulReviewAsync(Guid reviewId)
        {
            var review = await _IReviewRepository.GetReviewAsync(reviewId);
            if (review == null)
            {
               // return null;
            }

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            var isUseful = await _IReviewRepository.IsUsefulAsync(currentUserId, review.ReviewId);
            if (isUseful)
            {
               // return null;
            }

            _IReviewRepository.UnusfulReviewAsync(currentUserId, review.ReviewId);
            await _IReviewRepository.SaveChangesAsync();

            //var unusfulReviewToReturn = MapReview(currentUserId, review);
            //return unusfulReviewToReturn;
        }

        public async Task FunnyReviewAsync(Guid reviewId)
        {
            var review = await _IReviewRepository.GetReviewAsync(reviewId);
            if (review == null)
            {
               // return null;
            }

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            var isFunny = await _IReviewRepository.IsFunnyAsync(currentUserId, review.ReviewId);
            if (isFunny)
            {
                //return null;
            }

            await _IReviewRepository.FunnyReviewAsync(currentUserId, review.ReviewId);
            await _IReviewRepository.SaveChangesAsync();

           // var funnyReviewAsyncToReturn = MapReview(currentUserId, review);
           // return funnyReviewAsyncToReturn;
        }

        public async Task UnfunnyReviewAsync(Guid reviewId)
        {
            var review = await _IReviewRepository.GetReviewAsync(reviewId);
            if (review == null)
            {
                //return null;
            }

            var currentUserId = await _IUserService.GetCurrentUserIdAsync();

            var isFunny = await _IReviewRepository.IsFunnyAsync(currentUserId, review.ReviewId);
            if (isFunny)
            {
               // return null;
            }

            _IReviewRepository.UnfunnyReviewAsync(currentUserId, review.ReviewId);
            await _IReviewRepository.SaveChangesAsync();

          //  var unfunnyReviewAsyncToReturn = MapReview(currentUserId, review);
            //return unfunnyReviewAsyncToReturn;
        }
        private ReviewDto MapReview(Guid currentUserId, Review review)
        {
            var reviewDto = _mapper.Map<ReviewDto>(review);
            var profileDto = _mapper.Map<UserProfileDto>(review.User, a => a.Items["currentUserId"] = currentUserId);
            reviewDto.User = profileDto;

            return reviewDto;
        }
    }
}
