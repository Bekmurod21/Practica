using AutoMapper;
using Practic.Domain.Entities;
using Practic.Service.DTOs.Users;

namespace Practic.Service.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserForCreationDto>().ReverseMap();
            CreateMap<User, UserForResultDto>().ReverseMap();
            CreateMap<User, UserForUpdateDto>().ReverseMap();
        }

    }
}
