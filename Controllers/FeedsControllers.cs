using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Suma.Social.Repositories;

namespace Suma.Social.Controllers
{
    [ApiController]
    public class FeedsControllers : ControllerBase
    {
        private readonly INeoFeedRepository _neoFeedRepository;
        public FeedsControllers(INeoFeedRepository neoFeedRepository)
        {
            _neoFeedRepository = neoFeedRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var feeds = await _neoFeedRepository.GetAsync();
            return Ok(feeds);
        }
    }
}