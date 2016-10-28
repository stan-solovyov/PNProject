using System.Linq;
using AutoMapper;
using BLL.Models;
using Domain.Entities;

namespace PriceNotifier.DTO
{
    public class MyMappingProfile : Profile
    {
        public MyMappingProfile()
        {
            CreateMap<Product, ProductDto>().ForMember("Id", opt => opt.MapFrom(c => c.ProductId))
                                            .ForMember("Article", opt => opt.MapFrom(c => c.Articles.OrderByDescending(d => d.DateAdded).FirstOrDefault(a => a.IsPublished)));
            CreateMap<ProductDto, Product>();
            CreateMap<Product, Product>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserFromDbWithCount, UserDtoWithCount>();
            CreateMap<PriceHistory, PriceHistoryDto>();
            CreateMap<PriceHistoryDto, PriceHistory>();
            CreateMap<Article, ArticleDto>().ForMember("ProductName", opt => opt.MapFrom(c => c.Product.Name));
            CreateMap<ArticleDto, Article>();
        }
    }
}