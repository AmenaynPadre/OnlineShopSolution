using AutoMapper;
using OnlineShop.Api.Common;
using OnlineShop.Api.DTOs;
using OnlineShop.Api.Models;
using OnlineShop.Api.Repositories.Interfaces;
using OnlineShop.Api.Services.Interfaces;

namespace OnlineShop.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<Result> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.FailureResult("User not found.");
            }
            await _userRepository.DeleteAsync(userId);
            return Result.SuccessResult("User deleted successfully.");
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            if (!users.Any())
            {
                return Result<IEnumerable<UserDto>>.FailureResult("No users found.");
            }
            var userDtos = users.Select(user => _mapper.Map<UserDto>(user));

            return Result<IEnumerable<UserDto>>.SuccessResult(userDtos);
        }

        public async Task<Result<UserDto>> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) 
            {
                return Result<UserDto>.FailureResult("User not found.");
            }
            return Result<UserDto>.SuccessResult(_mapper.Map<UserDto>(user));
        }

        public async Task<Result> UpdateUserAsync(CreateUpdateUserDto userDto)
        {
            var user = await _userRepository.GetUserByUserNameAsync(userDto.UserName);

            if(user == null)
            {
                return Result.FailureResult("User not found.");
            }

            var updateResult = _userRepository.UpdateAsync(_mapper.Map(userDto, user));

            if(updateResult == null)
            {
                return Result.FailureResult("Error updating user.");
            }
            return Result.SuccessResult("User updated successfully.");
        }
    }
}
