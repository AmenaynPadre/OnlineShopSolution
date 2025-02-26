using Microsoft.EntityFrameworkCore;
using OnlineShop.Api.Data;
using OnlineShop.Api.Data.Entities;
using OnlineShop.Api.Repositories.Interfaces;

namespace OnlineShop.Api.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RoleRepository> _logger;

        public RoleRepository(AppDbContext context, ILogger<RoleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Role> FindByNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                _logger.LogError("Attempted to find a role with an empty or null name.");
                throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
            }

            try
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (role == null)
                {
                    _logger.LogWarning("Role with name '{RoleName}' not found.", roleName);
                }
                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving role with name '{RoleName}'.", roleName);
                throw;
            }
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.LogError("Attempted to retrieve a user with an empty or null username.");
                throw new ArgumentException("Username cannot be null or empty.", nameof(userName));
            }

            try
            {
                var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.UserName == userName);
                if (user == null)
                {
                    _logger.LogWarning("User with username '{UserName}' not found.", userName);
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user with username '{UserName}'.", userName);
                throw;
            }
        }

        public async Task AddAsync(Role role)
        {
            if (role == null)
            {
                _logger.LogError("Attempted to add a null role.");
                throw new ArgumentNullException(nameof(role), "Role cannot be null.");
            }

            try
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Role with ID {RoleId} added successfully.", role.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding role.");
                throw;
            }
        }
    }
}
