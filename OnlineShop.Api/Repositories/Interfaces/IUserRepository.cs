using OnlineShop.Api.Data.Entities;

namespace OnlineShop.Api.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByUserNameAsync(string userName);
    }
}
