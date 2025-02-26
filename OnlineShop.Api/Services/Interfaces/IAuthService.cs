using OnlineShop.Api.Data.Entities;
using OnlineShop.Api.DTOs;
using OnlineShop.Api.Models;

namespace OnlineShop.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> RegisterAsync(CreateUpdateUserDto request);
        Task<ServiceResponse<TokenResponseDto>> LoginAsync(LoginUserDto request);
        Task<ServiceResponse<TokenResponseDto>> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}
