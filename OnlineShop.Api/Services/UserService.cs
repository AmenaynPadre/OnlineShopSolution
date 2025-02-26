using AutoMapper;
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
        public async Task<ServiceResponse<string>> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ServiceResponse<string>.FailureResponse("User not found.");
            }
            await _userRepository.DeleteAsync(userId);
            return ServiceResponse<string>.SuccessResponse("User deleted successfully.");
        }

        public async Task<ServiceResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            if (!users.Any())
            {
                return ServiceResponse<IEnumerable<UserDto>>.FailureResponse("No users found.");
            }
            var userDtos = users.Select(user => _mapper.Map<UserDto>(user));

            return ServiceResponse<IEnumerable<UserDto>>.SuccessResponse(userDtos);
        }

        public async Task<ServiceResponse<UserDto>> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) 
            {
                return ServiceResponse<UserDto>.FailureResponse("User not found.");
            }
            return ServiceResponse<UserDto>.SuccessResponse(_mapper.Map<UserDto>(user));
        }

        public async Task<ServiceResponse<string>> UpdateUserAsync(CreateUpdateUserDto userDto)
        {
            var user = await _userRepository.GetUserByUserNameAsync(userDto.UserName);

            if(user == null)
            {
                return ServiceResponse<string>.FailureResponse("User not found.");
            }

            var updateResult = _userRepository.UpdateAsync(_mapper.Map(userDto, user));

            if(updateResult == null)
            {
                return ServiceResponse<string>.FailureResponse("Error updating user.");
            }
            return ServiceResponse<string>.SuccessResponse("User updated successfully.");
        }
    }
}
