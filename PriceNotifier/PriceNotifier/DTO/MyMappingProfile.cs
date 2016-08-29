using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Domain.Entities;

namespace PriceNotifier.DTO
{
    public class MyMappingProfile:Profile
    {
        public MyMappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<Product, Product>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}