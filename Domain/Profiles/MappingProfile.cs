using AutoMapper;
using Domain.DTOs;

namespace Domain.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Author, AuthorRead>();
        }
    }
}
