using AutoMapper;
using BabyMoo.DTOs.Auth;
using BabyMoo.DTOs.Product;
using BabyMoo.DTOs.Category;
using BabyMoo.Models;
using BabyMoo.DTOs.Cart;
using BabyMoo.DTOs.User;
using BabyMoo.DTOs.Address;
using BabyMoo.DTOs.Payment;
using BabyMoo.DTOs.Order;


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
            CreateMap<Payment, PaymentResultDto>()
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.Status == "PAID"))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.PaymentId.ToString()));
            CreateMap<Order, OrderViewDto>()
     .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
     .ForMember(dest => dest.AddressLine, opt => opt.MapFrom(src =>
         src.Address.Street + ", " + src.Address.City + ", " + src.Address.State + ", " + src.Address.PinCode + ", " + src.Address.Country))
     .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));


            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));
        }


       
        }
    }

