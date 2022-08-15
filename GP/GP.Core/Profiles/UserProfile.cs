using AutoMapper;
using RealWord.Data.Entities;
using RealWord.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GP.Core.Models;

namespace RealWord.Core.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserProfileDto>();

            CreateMap<UserLoginDto, User>();//done
            CreateMap<UserForCreationDto, User>();//done
            CreateMap<UserForUpdateDto, User>();//done if we change location we should change it
            CreateMap<UserForUpdatePasswordDto, User>();
        }
    }
}
