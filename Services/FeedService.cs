using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Suma.Social.Entities;
using Suma.Social.Models.Feed;
using Suma.Social.Repositories;

namespace Suma.Social.Services
{
    public interface IFeedService
    {
        Task<Feed> CreateAsync(CreateFeedRequest model, int userId);
        Task<IEnumerable<Feed>> GetListAsync(int postedUserId);
    }
    
    public class FeedService : IFeedService
    {
        private readonly INeoFeedRepository _neoFeedRepository;

        public FeedService(INeoFeedRepository neoFeedRepository)
        {
            _neoFeedRepository = neoFeedRepository;
        }

        public async Task<IEnumerable<Feed>> GetListAsync(int postedUserId)
        {
            return await _neoFeedRepository.GetAsync(postedUserId);
        }

        public async Task<Feed> CreateAsync(CreateFeedRequest model, int postedUserId)
        {
            var feed = new Feed 
            {
                Id = Guid.NewGuid().ToString(),
                Text = model.Text,
                PrivacyLevel = model.PrivacyLevel
            };
            return await _neoFeedRepository.InsertAsynce(feed, postedUserId);
        }
    }
}