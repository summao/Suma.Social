using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Suma.Social.Repositories;

namespace Suma.Social.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedsController : ControllerBase
    {
        private readonly INeoFeedRepository _neoFeedRepository;
        public FeedsController(INeoFeedRepository neoFeedRepository)
        {
            _neoFeedRepository = neoFeedRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var feeds = await _neoFeedRepository.GetAsync(1);
            return Ok(feeds);
        }
    }
}