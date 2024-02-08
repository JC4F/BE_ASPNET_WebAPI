using AutoMapper;
using BusinessObject.Dtos;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.AutoMappers
{
    public class ApplicationMapper: Profile
    {
        public ApplicationMapper()
        {
            CreateMap<User, UserDto>();
            CreateMap<Blog, BlogDto>();
            CreateMap<BlogRating, BlogRatingDto>();
        }
    }
}
