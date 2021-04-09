using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Suma.Social.Models.Posts;
using Suma.Social.Services;

namespace Suma.Social.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        const int userId = 1;
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        // fix to get friend's posts
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _postService.GetListAsync(userId);
            return Ok(posts);
        }

        [HttpGet("poster")]
        public async Task<IActionResult> GetByPoster()
        {
            var posts = await _postService.GetListAsync(userId);
            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePostRequest model)
        {
            var post = await _postService.CreateAsync(model, userId);
            return Created(post.Id, post);
        }
    }
}