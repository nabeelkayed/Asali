using Microsoft.EntityFrameworkCore;
using RealWord.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RealWord.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GPDbContext _context;

        public UserRepository(GPDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<bool> UserExistsAsync(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            bool userExists = await _context.Users.AnyAsync(u => u.Username == username);
            return userExists;
        }

        public async Task<bool> EmailAvailableAsync(string email)
        {
            var emailNotAvailable = await _context.Users.Select(a => a.Email).ContainsAsync(email);
            return emailNotAvailable;
        }

        public async Task<User> LoginUserAsync(User user)
        {
            var loginUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email
                                                             && u.Password == user.Password);
            return loginUser;
        }

        public async Task<User> GetUserAsync(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var user = await _context.Users.Include(u => u.Reviews)
                                           .Include(u => u.Followings)
                                           .Include(u => u.Cool)
                                           .Include(u => u.Funny)
                                           .Include(u => u.Useful)
                                           .FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public async Task<User> GetUserAsNoTrackingAsync(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var user = await _context.Users.AsNoTracking()
                                           .Include(u => u.Reviews)
                                           .Include(u => u.Followings)
                                           .Include(u => u.Cool)
                                           .Include(u => u.Funny)
                                           .Include(u => u.Useful)
                                           .FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void UpdateUser(User updatedUser, User userForUpdate)
        {

            if (!string.IsNullOrWhiteSpace(userForUpdate.FirstName))
            {
                updatedUser.FirstName = userForUpdate.FirstName.ToLower();
            }

            if (!string.IsNullOrWhiteSpace(userForUpdate.LastName))
            {
                updatedUser.LastName = userForUpdate.LastName.ToLower();
            }

            if (!string.IsNullOrWhiteSpace(userForUpdate.Username))
            {
                updatedUser.Username = userForUpdate.Username.ToLower();
            }

            if (!string.IsNullOrWhiteSpace(userForUpdate.Email))
            {
                updatedUser.Email = userForUpdate.Email.ToLower();
            }

            if (!string.IsNullOrWhiteSpace(userForUpdate.Photo))
            {
                updatedUser.Photo = userForUpdate.Photo;
            }

            if (!string.IsNullOrWhiteSpace(userForUpdate.Bio))
            {
                updatedUser.Bio = userForUpdate.Bio;
            }

            if (!string.IsNullOrWhiteSpace(userForUpdate.HeadLine))
            {
                updatedUser.HeadLine = userForUpdate.HeadLine.ToLower();
            }

            if (!string.IsNullOrWhiteSpace(userForUpdate.Location))
            {
                updatedUser.Location = userForUpdate.Location.ToLower();
            }
        }

        public void UpdateUserPassword(User updatedUser)
        {

        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}