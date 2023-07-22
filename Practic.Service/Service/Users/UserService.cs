using AutoMapper;
using Practic.Domain.Entities;
using Practic.Service.Helpers;
using Practic.Service.DTOs.Users;
using Practic.Data.IRepositories;
using Practic.Service.Exceptions;
using Practic.Service.Extensions;
using Practic.Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using Practic.Service.Interfaces.Users;

namespace Practic.Service.Service.Users
{
    public class UserService : IUserService
    {
        private readonly IMapper mapper;
        private readonly IRepository<User> repository;
        public UserService(IRepository<User> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<UserForResultDto> AddAsync(UserForCreationDto dto)
        {
            var existUser = await repository.SelectAsync(p => p.Phone == dto.Phone);
            if (existUser != null && !existUser.IsDeleted)
                throw new CustomException(409, "User already exist");

            var mapped = mapper.Map<User>(dto);
            mapped.CreatedAt = DateTime.UtcNow;
            mapped.Password = PasswordHelper.Hash(dto.Password);
            var addedModel = await repository.InsertAsync(mapped);
            await repository.SaveAsync();

            return mapper.Map<UserForResultDto>(addedModel);
        }

        public async Task<UserForResultDto> ChangePasswordAsync(UserForChangePasswordDto dto)
        {
            var user = await repository.SelectAsync(u => u.Email == dto.Email);
            if (user is null || user.IsDeleted)
                throw new CustomException(404, "User not found!");

            if (!PasswordHelper.Verify(dto.OldPassword, user.Password))
                throw new CustomException(400, "Password is incorrect");

            if (dto.NewPassword != dto.ComfirmPassword)
                throw new CustomException(400, "New password and confirm password are not equal");

            user.Password = PasswordHelper.Hash(dto.NewPassword);
            user.UpdatedBy = HttpContextHelper.UserId;
            await repository.SaveAsync();

            return mapper.Map<UserForResultDto>(user);
        }

        public async Task<UserForResultDto> ModifyAsync(long id, UserForUpdateDto dto)
        {
            var user = await repository.SelectAsync(u => u.Id == id);
            if (user is null || user.IsDeleted)
                throw new CustomException(404, "Couldn't found user for given Id");

            var modifiedUser = mapper.Map(dto, user);
            modifiedUser.UpdatedAt = DateTime.UtcNow;
            modifiedUser.UpdatedBy = HttpContextHelper.UserId;

            await repository.SaveAsync();

            return mapper.Map<UserForResultDto>(user);
        }

        public async Task<bool> RemoveAsync(long id)
        {
            var user = await repository.SelectAsync(u => u.Id == id);
            if (user is null || user.IsDeleted)
                throw new CustomException(404, "Couldn't find user for this given Id");

            var accessor = HttpContextHelper.Accessor;
            user.DeletedBy = HttpContextHelper.UserId;

            await repository.DeleteAsync(u => u.Id == id);

            await repository.SaveAsync();

            return true;
        }

        public async Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(PaginationParams @params)
        {
            var users = await repository.SelectAll()
            .Where(u => u.IsDeleted == false)
            .ToPagedList(@params)
            .ToListAsync();

            return mapper.Map<IEnumerable<UserForResultDto>>(users);
        }

        public async Task<User> RetrieveByEmailAsync(string email)
         => await repository.SelectAsync(u => u.Email == email);

        public async Task<UserForResultDto> RetrieveByIdAsync(long id)
        {
            var user = await repository.SelectAsync(u => u.Id == id);
            if (user is null || user.IsDeleted)
                throw new CustomException(404, "User Not Found");

            return mapper.Map<UserForResultDto>(user);
        }
    }
}
