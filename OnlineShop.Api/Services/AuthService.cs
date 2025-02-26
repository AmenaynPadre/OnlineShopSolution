using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Api.Common;
using OnlineShop.Api.Data.Entities;
using OnlineShop.Api.DTOs;
using OnlineShop.Api.Models;
using OnlineShop.Api.Repositories.Interfaces;
using OnlineShop.Api.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OnlineShop.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(IUserRepository userRepository,
            IRoleRepository roleRepository,
            IMapper mapper,
            IConfiguration configuration,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<Result<TokenResponseDto>> LoginAsync(LoginUserDto request)
        {
            var user = await _userRepository.GetUserByUserNameAsync(request.UserName);

            if(user is null)
            {
                return Result<TokenResponseDto>.FailureResult("User not found.");
            }
            if(_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return Result<TokenResponseDto>.FailureResult("Invalid password.");
            }

            var token = await CreateTokenResponse(user);

            return Result<TokenResponseDto>.SuccessResult(token);
        }

        public async Task<Result> RegisterAsync(CreateUpdateUserDto request)
        {
            var existingUser = await _userRepository.GetUserByUserNameAsync(request.UserName);

            if (existingUser != null) 
            {
                return Result.FailureResult("User with this username already exists.");
            }

            var role = await _roleRepository.FindByNameAsync("User");
            if (role == null)
            {
                return Result.FailureResult("Role not found.");
            }

            var user = _mapper.Map<User>(request);

            user.Roles = new List<Role> { role };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepository.AddAsync(user);

            return Result.SuccessResult("User registered successfully.");

        }

        private async Task<TokenResponseDto> CreateTokenResponse(User? user)
        {
            return new TokenResponseDto
            {
                AccessToken = await CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        private async Task<string> CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
                };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = await Task.Run(() =>
            {
                var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Token"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationInMinutes"])),
                    Audience = _configuration["JwtSettings:Audience"],
                    Issuer = _configuration["JwtSettings:Issuer"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);
            return refreshToken;
        }

        public async Task<Result<TokenResponseDto>> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
                return Result<TokenResponseDto>.FailureResult("aaaaaaa");

            var token = await CreateTokenResponse(user);

            return Result<TokenResponseDto>.SuccessResult(token);
        }

        private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }
    }
}
