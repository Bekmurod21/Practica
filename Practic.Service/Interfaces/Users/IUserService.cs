using Practic.Domain.Configurations;
using Practic.Domain.Entities;
using Practic.Service.DTOs.Users;

namespace Practic.Service.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserForResultDto> AddAsync(UserForCreationDto dto);
        Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(PaginationParams @params);
        Task<User> RetrieveByEmailAsync(string email);
        Task<bool> RemoveAsync(long id);
        Task<UserForResultDto> RetrieveByIdAsync(long id);
        Task<UserForResultDto> ModifyAsync(long id, UserForUpdateDto dto);
        Task<UserForResultDto> ChangePasswordAsync(UserForChangePasswordDto dto);
    }
}
