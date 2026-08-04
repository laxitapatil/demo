using AutoMapper;
using Core.Domain;
using Core.Enumeration;
using Core.Request;
using Core.Response;
namespace Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRequest, AspNetUsers>()
                .ForMember(destionation => destionation.PasswordHash, option => option.MapFrom(source => source.Password));
            CreateMap<AspNetUsers, UserResponse>();
            CreateMap<AspNetUsers, LoginResponse>();
            CreateMap<MenuRequest, Menu>();
            CreateMap<MenuRightsRequest, MenuRights>();
            CreateMap<BlogCategoryRequest, BlogCategory>();
            CreateMap<BlogRequest, Blog>();
            CreateMap<AppointmentRequest, Appointment>();
        }
    }
}