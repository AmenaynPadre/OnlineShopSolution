using OnlineShop.Api.Models;
using OnlineShop.Api.DTOs;
using OnlineShop.Api.Common;

namespace OnlineShop.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserDto>> GetUserByIdAsync(int userId);
        Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<Result> UpdateUserAsync(CreateUpdateUserDto userDto);
        Task<Result> DeleteUserAsync(int userId);
    }
}
