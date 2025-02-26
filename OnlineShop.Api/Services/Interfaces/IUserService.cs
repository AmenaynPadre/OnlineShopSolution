using OnlineShop.Api.Models;
using OnlineShop.Api.DTOs;

namespace OnlineShop.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<UserDto>> GetUserByIdAsync(int userId);
        Task<ServiceResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ServiceResponse<string>> UpdateUserAsync(CreateUpdateUserDto userDto);
        Task<ServiceResponse<string>> DeleteUserAsync(int userId);
    }
}
