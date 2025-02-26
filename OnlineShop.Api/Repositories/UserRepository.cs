using Microsoft.EntityFrameworkCore;
using OnlineShop.Api.Data;
using OnlineShop.Api.Data.Entities;
using OnlineShop.Api.Repositories.Interfaces;

namespace OnlineShop.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddAsync(User user)
        {
            if (user == null)
            {
                _logger.LogError("Attempted to add a null user.");
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User with ID {UserId} added successfully.", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user.");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found for deletion.", id);
                    return;
                }

                _context.Roles.RemoveRange(user.Roles);

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User with ID {UserId} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with ID {UserId}.", id);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all users.");
                throw;
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", id);
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user with ID {UserId}.", id);
                throw;
            }
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.LogError("Attempted to retrieve user with empty or null username.");
                throw new ArgumentException("Username cannot be null or empty.", nameof(userName));
            }

            try
            {
                var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.UserName == userName);
                if (user == null)
                {
                    _logger.LogWarning("User with username {UserName} not found.", userName);
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user with username {UserName}.", userName);
                throw;
            }
        }

        public async Task UpdateAsync(User user)
        {
            if (user == null)
            {
                _logger.LogError("Attempted to update a null user.");
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User with ID {UserId} updated successfully.", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user with ID {UserId}.", user.Id);
                throw;
            }
        }
    }
}
