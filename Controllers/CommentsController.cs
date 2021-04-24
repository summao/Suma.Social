using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Suma.Social.Exceptions;
using Suma.Social.Models.Comments;
using Suma.Social.Services;
using Suma.Social.Utils;

namespace Suma.Social.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetMany(string postId)
        {
            try
            {
                var comments = await _commentService.GetManyAsync(postId, User.GetUserId());
                return Ok(comments);
            }
            catch (NoPermissionException)
            {
                return Unauthorized("do not have permission to see these comments");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest model)
        {
            var result = await _commentService.CreateAsync(model, User.GetUserId());
            return Created(result.CommentId, result);
        }
    }
}