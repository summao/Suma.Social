using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Suma.Social.Models.Comments;
using Suma.Social.Services;

namespace Suma.Social.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        const int userId = 1;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest model)
        {
            var result = await _commentService.CreateAsync(model, userId);
            return Created(result.CommentId, result);
        }
    }
}