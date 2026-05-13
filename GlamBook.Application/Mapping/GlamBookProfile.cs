using AutoMapper;
using GlamBook.Application.DTOs;
using GlamBook.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GlamBook.Application.Mapping
{
    public class GlamBookProfile : Profile
    {
        public GlamBookProfile()
        {
            // Salon <-> SalonDto
            CreateMap<Salon, SalonDto>().ReverseMap();

            // Service <-> ServiceDto
            CreateMap<Service, ServiceDto>()
                .ForMember(d => d.DurationInMinutes, o => o.MapFrom(s => (int)s.Duration.TotalMinutes));
            CreateMap<ServiceDto, Service>()
                .ForMember(d => d.Duration, o => o.MapFrom(s => TimeSpan.FromMinutes(s.DurationInMinutes)));

            // Appointment -> AppointmentDto
            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.SalonName, opt => opt.MapFrom(src => src.Salon.Name))
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name));


            // Rating <-> RatingDto
            CreateMap<Rating, RatingDto>().ReverseMap();

            // BlogPost <-> BlogPostDto
            CreateMap<BlogPost, BlogPostDto>().ReverseMap();

            // Category <-> CategoryDto
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
