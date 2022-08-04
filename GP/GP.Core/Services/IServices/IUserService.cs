using GP.Core.Models;
using RealWord.Core.Models;
using System;
using System.Threading.Tasks;

namespace RealWord.Core.Services
{
    public interface IUserService
    {
        Task<UserDto> LoginUserAsync(UserLoginDto userLogin);
        Task<UserProfileDto> GetCurrentUserAsync();
        Task<Guid> GetCurrentUserIdAsync();
        Task<UserProfileDto> GetUserProfileAsync(string username);
        Task CreateUserAsync(UserForCreationDto userForCreation);
        Task UpdateUserAsync(UserForUpdateDto userForUpdate);
        Task UpdateUserPasswordAsync(UserForUpdatePasswordDto userForUpdatePassword);
    }
}