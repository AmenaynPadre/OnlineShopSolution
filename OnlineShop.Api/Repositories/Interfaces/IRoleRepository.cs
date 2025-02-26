using OnlineShop.Api.Data.Entities;

namespace OnlineShop.Api.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> FindByNameAsync(string roleName);
        Task AddAsync(Role role);
        Task<User> GetByUserNameAsync(string userName);
    }
}
