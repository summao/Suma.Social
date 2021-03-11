using System;
using System.Threading.Tasks;
using Suma.Social.Entities;
using Suma.Social.Models.Comments;
using Suma.Social.Repositories;

namespace Suma.Social.Services
{
    public interface ICommentService
    {
        Task<CreateCommentResponse> CreateAsync(CreateCommentRequest model, int commentorId);
    }

    public class CommentService : ICommentService
    {
        private readonly INeoCommentRepository _neoCommentRepository;

        public CommentService(INeoCommentRepository neoCommentRepository)
        {
            _neoCommentRepository = neoCommentRepository;
        }

        public async Task<CreateCommentResponse> CreateAsync(CreateCommentRequest model, int commentorId)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Text = model.Text,
            };
            return await _neoCommentRepository.InsertAsync(comment, commentorId, model.replyToFeedId);
        }
    }
}