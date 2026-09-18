using Core.Request;
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
            CreateMap<NewsCategoryRequest, NewsCategory>();
            CreateMap<NewsRequest, News>();
            CreateMap<AppointmentRequest, Appointment>();
            CreateMap<MediaRequest, Media>();
            CreateMap<ContactusEnquiryRequest, ContactusEnquiry>();
            CreateMap<TestimonialRequest, Testimonial>();
            CreateMap<SubscriberRequest, Subscriber>();
        }
    }
}