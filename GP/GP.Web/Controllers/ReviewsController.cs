using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealWord.Core.Models;
using RealWord.Core.Services;
using RealWord.Utils.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/businesses")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _IReviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _IReviewService = reviewService ??
                throw new ArgumentNullException(nameof(reviewService));
        }

        [AllowAnonymous] //othenticational optinal or just in the business side
        [HttpGet("{businessId}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForBusiness(Guid businessId)
        {
            var reviewsToReturn = await _IReviewService.GetReviewsForBusinessAsync(businessId);
            if (reviewsToReturn == null)
            {
                return NotFound();
            }

            return Ok(reviewsToReturn);
        }

        [HttpGet("feedreviews")]//shoud go to the user controler
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetFeedReviews([FromQuery] FeedReviewsParameters feedReviewsParameters)
        {
            var reviewsToReturn = await _IReviewService.GetFeedReviewsAsync(feedReviewsParameters);
            if (reviewsToReturn == null)
            {
                return NotFound();
            }

            return Ok(new { reviews = reviewsToReturn, reviewsCount = reviewsToReturn.Count() });
        }

        [HttpPost("{businessId}/reviews")]
        public async Task<IActionResult> CreateReviewForBusiness(Guid businessId, ReviewForCreationDto reviewForCreationDto)
        {
            await _IReviewService.CreateReviewForBusinessAsync(businessId, reviewForCreationDto);
            return Ok();
        }

        [HttpDelete("{businessId}/reviews/{reviewId}")]
        public async Task<IActionResult> DeleteReview(Guid businessId, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessId, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            var isAuthorized = await _IReviewService.IsAuthorized(businessId, reviewId);
            if (!isAuthorized)
            {
                return Forbid();
            }

            await _IReviewService.DeleteReviewAsync(reviewId);
            return NoContent();
        }

        [HttpPost("{businessId}/reviews/{reviewId}/cool")]
        public async Task<IActionResult> CoolReview(Guid businessId, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessId, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            await _IReviewService.CoolReviewAsync(reviewId);
            return Ok();
        }

        [HttpDelete("{businessId}/reviews/{reviewId}/cool")]
        public async Task<IActionResult> UnCoolReview(Guid businessId, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessId, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            await _IReviewService.UncoolReviewAsync(reviewId);
            return Ok();
        }

        [HttpPost("{businessId}/reviews/{reviewId}/useful")]
        public async Task<IActionResult> UsefulReview(Guid businessId, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessId, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            await _IReviewService.UsefulReviewAsync(reviewId);
            return Ok();
        }

        [HttpDelete("{businessId}/reviews/{reviewId}/useful")]
        public async Task<IActionResult> UnUsefulReview(Guid businessId, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessId, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            await _IReviewService.UnusefulReviewAsync(reviewId);
            return Ok();
        }

        [HttpPost("{businessId}/reviews/{reviewId}/funny")]
        public async Task<IActionResult> FunnyReview(Guid businessId, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessId, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            await _IReviewService.FunnyReviewAsync(reviewId);
            return Ok();
        }

        [HttpDelete("{businessId}/reviews/{reviewId}/funny")]
        public async Task<IActionResult> UnFunnyReview(Guid businessId, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessId, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            await _IReviewService.UnfunnyReviewAsync(reviewId);
            return Ok();
        }
    }
}