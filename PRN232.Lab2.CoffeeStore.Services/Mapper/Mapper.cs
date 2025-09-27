using AutoMapper;
using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Services.RequestModel;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<ProductRequest, Product>();

            CreateMap<User, UserResponse>();
            CreateMap<RegisterRequest, User>();

            CreateMap<OrderRequest, Order>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src=>src.Items));
            
            CreateMap<OrderItemRequest, OrderDetail>();

            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.UserResponse, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.OrderDetailResponses, opt => opt.MapFrom(src => src.OrderDetails))
                .ForMember(dest => dest.PaymentResponse, opt => opt.MapFrom(src => src.Payment));

            CreateMap<OrderDetail, OrderDetailResponse>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
            CreateMap<Payment, PaymentResponse>();
        }
    }
}
