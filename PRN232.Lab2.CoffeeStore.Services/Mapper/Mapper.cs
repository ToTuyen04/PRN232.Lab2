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
            //CreateMap<IEnumerable<Product>, IEnumerable<ProductResponse>>()
            //    .ForMember(dest => dest, opt => opt.MapFrom(src => src));

            CreateMap<ProductRequest, Product>();
        }
    }
}
