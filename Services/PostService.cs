using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Suma.Social.Entities;
using Suma.Social.Models.Posts;
using Suma.Social.Repositories;

namespace Suma.Social.Services
{
    public interface IPostService
    {
        Task<Post> CreateAsync(CreatePostRequest model, int userId);
        Task<IEnumerable<Post>> GetListAsync(int postedUserId);
    }

    public class PostService : IPostService
    {
        private readonly INeoPostRepository _neoPostRepository;

        public PostService(INeoPostRepository neoPostRepository)
        {
            _neoPostRepository = neoPostRepository;
        }

        public async Task<IEnumerable<Post>> GetListAsync(int postedUserId)
        {
            return await _neoPostRepository.GetAsync(postedUserId);
        }

        public async Task<Post> CreateAsync(CreatePostRequest model, int postedUserId)
        {
            var post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                Text = model.Text,
                PrivacyLevel = model.PrivacyLevel
            };
            return await _neoPostRepository.InsertAsynce(post, postedUserId);
        }
    }
}