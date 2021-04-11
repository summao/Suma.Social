using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Suma.Social.Repositories;

namespace Suma.Social.Services
{
    public interface IPersonService
    {
        /// <returns>A string of profile image name</return>
        Task<string> UpdateProfileImageAsync(IFormFile profileImage, int personId);
    }

    public class PersonService : IPersonService
    {
        private readonly IImageService _imageService;
        private readonly IPersonNeoRepository _personNeoRepository;

        public PersonService(
            IImageService imageService,
            IPersonNeoRepository personNeoRepository
        )
        {
            _imageService = imageService;
            _personNeoRepository = personNeoRepository;
        }

        public async Task<string> UpdateProfileImageAsync(IFormFile profileImage, int personId)
        {
            var profileImageName = await _imageService.AddOneAsync(profileImage);
            await _personNeoRepository.UpdateProfileImageNameAsync(profileImageName, personId);

            return profileImageName;
        }
    }
}