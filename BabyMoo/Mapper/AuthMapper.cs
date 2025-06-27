using AutoMapper;
using BabyMoo.DTOs.Auth;
using BabyMoo.DTOs.Product;
using BabyMoo.DTOs.Category;
using BabyMoo.Models;
using BabyMoo.DTOs.Cart;
using BabyMoo.DTOs.User;
using BabyMoo.DTOs.Address;

namespace BabyMoo.Mapper
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<Register, User>().ReverseMap();
            CreateMap<Login, User>().ReverseMap();
            CreateMap<Category, CategoryViewDto>().ReverseMap();
            CreateMap<Product, ProductViewDto>()
                           .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));

            CreateMap<ProductDto, Product>();

            CreateMap<CartItem, CartItemDto>()
               .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
               .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl));
            CreateMap<User, UserViewDto>();
            CreateMap<CreateAddressDto, Address>();
            CreateMap<Address, AddressDto>().ReverseMap();


            CreateMap<User, ResultDto>();
        }
    }
}
