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
        private readonly IImageService _imageService;

        public PostService(
            INeoPostRepository neoPostRepository,
            IImageService imageService
        )
        {
            _neoPostRepository = neoPostRepository;
            _imageService = imageService;
        }

        public async Task<IEnumerable<Post>> GetListAsync(int postedUserId)
        {
            return await _neoPostRepository.GetAsync(postedUserId);
        }

        public async Task<Post> CreateAsync(CreatePostRequest model, int postedUserId)
        {
            string imageName = null;
            if (model.Image != null)
            {
                imageName = await _imageService.AddOneAsync(model.Image);
            }

            var post = new Post
            {
                Id = Guid.NewGuid().ToString(),
                PrivacyLevel = model.PrivacyLevel,
                Text = model.Text,
                ImageName = imageName,
            };
            return await _neoPostRepository.InsertAsynce(post, postedUserId);
        }
    }
}