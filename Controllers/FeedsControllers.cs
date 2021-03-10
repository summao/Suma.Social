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
        public FeedsController(IFeedService feedService)
        {
            _feedService = feedService;
        }

        // [HttpGet]
        // public async Task<IActionResult> Get()
        // {
        //     var feeds = await _neoFeedRepository.GetAsync(1);
        //     return Ok(feeds);
        // }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFeedRequest model)
        {
            var userId = 1;
            var feed = await _feedService.CreateAsync(model, userId);
            return Created(feed.Id, feed);
        }
    }
}