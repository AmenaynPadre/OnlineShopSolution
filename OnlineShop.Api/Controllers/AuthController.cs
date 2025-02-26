using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Api.DTOs;
using OnlineShop.Api.Services;
using OnlineShop.Api.Services.Interfaces;

namespace OnlineShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginUserDto request)
        {
            var response = await _authService.LoginAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(CreateUpdateUserDto request)
        {
            var response = await _authService.RegisterAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokensAsync(request);

            if (result == null || result.Data == null)
                return Unauthorized("Invalid refresh token.");

            if (result.Data.AccessToken == null || result.Data.RefreshToken == null)
                return Unauthorized("Invalid refresh token.");

            return Ok(result);
        }
    }
}
