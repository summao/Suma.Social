using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Suma.Social.Models.Posts;
using Suma.Social.Services;
using Suma.Social.Utils;

namespace Suma.Social.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        // fix to get friend's posts
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _postService.GetListAsync(PersonsController.userId);
            return Ok(posts);
        }

        [HttpGet("poster")]
        public async Task<IActionResult> GetByPoster()
        {
            var posts = await _postService.GetListAsync(User.GetUserId());
            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePostRequest model)
        {
            var post = await _postService.CreateAsync(model, User.GetUserId());
            return Created(post.Id, post);
        }
    }
}