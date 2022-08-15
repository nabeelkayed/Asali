using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealWord.Data.Entities;
using RealWord.Core.Models;

namespace RealWord.Core.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>()
               .ForMember(
                    dest => dest.FunnyCount,
                    opt => opt.MapFrom(src => src.Funny.Count()))
                 .ForMember(
                    dest => dest.CoolCount,
                    opt => opt.MapFrom(src => src.Cool.Count()))
                  .ForMember(
                    dest => dest.UsefulCount,
                    opt => opt.MapFrom(src => src.Useful.Count()));

            CreateMap<ReviewForCreationDto, Review>();
        }
    }
}