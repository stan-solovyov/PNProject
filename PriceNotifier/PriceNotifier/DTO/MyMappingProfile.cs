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
            CreateMap<Product, ProductDto>().ForMember(a=>a.Id, opt => opt.MapFrom(c => c.ProductId))
                                            .ForMember(a=>a.Article, opt => opt.MapFrom(c => c.Articles.OrderByDescending(d => d.DateAdded).FirstOrDefault(a => a.IsPublished)))
                                            .ForMember(a=>a.ImageUrl, opt => opt.MapFrom(c => c.ProvidersProductInfos.First().ImageUrl))
                                            .ForMember(a=>a.MaxPrice, opt => opt.MapFrom(c => c.ProvidersProductInfos.Max(a => a.MaxPrice)))
                                            .ForMember(a=>a.MinPrice, opt => opt.MapFrom(c => c.ProvidersProductInfos.Min(a => a.MinPrice)));
            CreateMap<ProductDto, Product>();
            CreateMap<Product, Product>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserFromDbWithCount, UserDtoWithCount>();
            CreateMap<PriceHistory, PriceHistoryDto>();
            CreateMap<PriceHistoryDto, PriceHistory>();
            CreateMap<Article, ArticleDto>().ForMember(a=>a.ProductName, opt => opt.MapFrom(c => c.Product.Name))
                                            .ForMember(a => a.ProductUrl, opt => opt.MapFrom(c => c.Product.ProvidersProductInfos.FirstOrDefault().Url));
            CreateMap<ArticleDto, Article>();
        }
    }
}