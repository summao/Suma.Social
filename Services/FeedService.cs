using System;
using System.Threading.Tasks;
using Suma.Social.Entities;
using Suma.Social.Models.Feed;
using Suma.Social.Repositories;

namespace Suma.Social.Services
{
    public interface IFeedService
    {
        Task<Feed> CreateAsync(CreateFeedRequest model, int userId);
    }
    
    public class FeedService : IFeedService
    {
        private readonly INeoFeedRepository _neoFeedRepository;

        public FeedService(INeoFeedRepository neoFeedRepository)
        {
            _neoFeedRepository = neoFeedRepository;
        }

        public async Task<Feed> CreateAsync(CreateFeedRequest model, int userId){
            var feed = new Feed 
            {
                Id = Guid.NewGuid().ToString(),
                Text = model.Text,
                PrivacyLevel = model.PrivacyLevel
            };
            return await _neoFeedRepository.InsertAsynce(feed, userId);
        }
    }
}