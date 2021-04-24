using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Suma.Social.Entities;
using Suma.Social.Exceptions;
using Suma.Social.Models.Comments;
using Suma.Social.Repositories;

namespace Suma.Social.Services
{
    public interface ICommentService
    {
        Task<List<Comment>> GetManyAsync(string postId, int userId);
        Task<CreateCommentResponse> CreateAsync(CreateCommentRequest model, int commentorId);
    }

    public class CommentService : ICommentService
    {
        private readonly INeoCommentRepository _neoCommentRepository;
        private readonly IPostService _postService;

        public CommentService(
            INeoCommentRepository neoCommentRepository,
            IPostService postService
        )
        {
            _neoCommentRepository = neoCommentRepository;
            _postService = postService;
        }

        public async Task<List<Comment>> GetManyAsync(string postId, int userId)
        {
            var post = await _postService.GetOneByPostIdAsync(postId);
            if (post.PrivacyLevel != "public")
            {
                if (post.PosterId != userId)
                {
                    throw new NoPermissionException();
                }
            }

            return await _neoCommentRepository.GetManyAsync(postId);
        }

        public async Task<CreateCommentResponse> CreateAsync(CreateCommentRequest model, int commentorId)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Text = model.Text,
            };
            return await _neoCommentRepository.InsertAsync(comment, commentorId, model.ReplyToPostId);
        }
    }
}