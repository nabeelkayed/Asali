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
        Task <bool> CreateUserAsync(UserForCreationDto userForCreation);
        Task<bool> UpdateUserAsync(UserForUpdateDto userForUpdate);
        Task<bool> UpdateUserPasswordAsync(UserForUpdatePasswordDto userForUpdatePassword);
    }
}