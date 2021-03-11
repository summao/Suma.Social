using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Suma.Social.Models.Feed;
using Suma.Social.Services;

namespace Suma.Social.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedsController : ControllerBase
    {
        private readonly IFeedService _feedService;

        const int userId = 1;
        public FeedsController(IFeedService feedService)
        {
            _feedService = feedService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var feeds = await _feedService.GetListAsync(userId);
            return Ok(feeds);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFeedRequest model)
        {
            var feed = await _feedService.CreateAsync(model, userId);
            return Created(feed.Id, feed);
        }
    }
}