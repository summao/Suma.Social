using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Suma.Social.Entities;
using Suma.Social.Models.Comments;
using Suma.Social.Repositories;

namespace Suma.Social.Services
{
    public interface ICommentService
    {
        Task<List<Comment>> GetManyAsync(string postId);
        Task<CreateCommentResponse> CreateAsync(CreateCommentRequest model, int commentorId);
    }

    public class CommentService : ICommentService
    {
        private readonly INeoCommentRepository _neoCommentRepository;

        public CommentService(INeoCommentRepository neoCommentRepository)
        {
            _neoCommentRepository = neoCommentRepository;
        }

        public async Task<List<Comment>> GetManyAsync(string postId)
        {
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