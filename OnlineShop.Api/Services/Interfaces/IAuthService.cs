using OnlineShop.Api.Common;
using OnlineShop.Api.Data.Entities;
using OnlineShop.Api.DTOs;
using OnlineShop.Api.Models;

namespace OnlineShop.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(CreateUpdateUserDto request);
        Task<Result<TokenResponseDto>> LoginAsync(LoginUserDto request);
        Task<Result<TokenResponseDto>> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}
