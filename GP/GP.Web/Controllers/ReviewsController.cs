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
        [HttpGet("{businessUsername}/reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviewsForBusiness(string businessUsername)
        {
            var reviewsToReturn = await _IReviewService.GetReviewsForBusinessAsync(businessUsername);
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

        [HttpPost("{businessUsername}/reviews")]
        public async Task<IActionResult> CreateReviewForBusiness(string businessUsername, ReviewForCreationDto reviewForCreationDto)
        {
            await _IReviewService.CreateReviewForBusinessAsync(businessUsername, reviewForCreationDto);
            return Ok();
        }

        [HttpDelete("{businessUsername}/reviews/{reviewId}")]
        public async Task<IActionResult> DeleteReview(string businessUsername, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessUsername, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            var isAuthorized = await _IReviewService.IsAuthorized(businessUsername, reviewId);
            if (!isAuthorized)
            {
                return Forbid();
            }

            await _IReviewService.DeleteReviewAsync(reviewId);
            return NoContent();
        }

        [HttpPost("{businessUsername}/reviews/{reviewId}/cool")]
        public async Task<IActionResult> CoolReview(string businessUsername, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessUsername, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            await _IReviewService.CoolReviewAsync(reviewId);
            return Ok();
        }

        [HttpDelete("{businessUsername}/reviews/{reviewId}/cool")]
        public async Task<IActionResult> UnCoolReview(string businessUsername, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessUsername, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            await _IReviewService.UncoolReviewAsync(reviewId);
            return Ok();
        }

        [HttpPost("{businessUsername}/reviews/{reviewId}/useful")]
        public async Task<IActionResult> UsefulReview(string businessUsername, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessUsername, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            await _IReviewService.UsefulReviewAsync(reviewId);
            return Ok();
        }

        [HttpDelete("{businessUsername}/reviews/{reviewId}/useful")]
        public async Task<IActionResult> UnUsefulReview(string businessUsername, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessUsername, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            await _IReviewService.UnusefulReviewAsync(reviewId);
            return Ok();
        }

        [HttpPost("{businessUsername}/reviews/{reviewId}/funny")]
        public async Task<IActionResult> FunnyReview(string businessUsername, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessUsername, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            await _IReviewService.FunnyReviewAsync(reviewId);
            return Ok();
        }

        [HttpDelete("{businessUsername}/reviews/{reviewId}/funny")]
        public async Task<IActionResult> UnFunnyReview(string businessUsername, Guid reviewId)
        {
            var isReviewExists = await _IReviewService.ReviewExistsAsync(businessUsername, reviewId);
            if (!isReviewExists)
            {
                return NotFound();
            }

            await _IReviewService.UnfunnyReviewAsync(reviewId);
            return Ok();
        }
    }
}