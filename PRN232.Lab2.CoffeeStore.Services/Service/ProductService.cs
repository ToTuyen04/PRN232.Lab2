using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository;
using PRN232.Lab2.CoffeeStore.Services.ExceptionHandler;
using PRN232.Lab2.CoffeeStore.Services.Helpers;
using PRN232.Lab2.CoffeeStore.Services.RequestModel;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using PRN232.Lab2.CoffeeStore.Services.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Add(Product obj)
        {
            _unitOfWork.Product.Add(obj);
        }

        public async Task<ProductResponse> AddAsync(ProductRequest obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "ProductRequest object cannot be null.");

            //if (CheckProductNameExists(obj.Name))
            bool check = HelperClass.CheckDuplicatedName(obj.Name, _unitOfWork.Product.GetAll());
            if (check)
                throw new ExceptionHandler.ValidationException($"Product name '{obj.Name}' already exists.");

            var p = _mapper.Map<Product>(obj);
            await _unitOfWork.Product.AddAsync(p);
            await _unitOfWork.SaveAsync();

            var loadedPro = await _unitOfWork.Product.GetAsync(pr => pr.ProductId == p.ProductId);
            var response = _mapper.Map<ProductResponse>(p);
            return response;
        }

        public void Delete(Product obj)
        {
            _unitOfWork.Product.Remove(obj);
        }

        public async Task DeleteAsync(Product obj)
        {
            _unitOfWork.Product.Remove(obj);
            await _unitOfWork.SaveAsync();
        }

        public IEnumerable<Product> GetAll()
        {
            return _unitOfWork.Product.GetAll();
        }

        public async Task<Paginated<object>> GetAllAsync(string search, int currentPage, int pageSize, string orderBy, string select)
        {
            var (dynamicItems, totalCount) = await _unitOfWork.Product.GetPaginatedAsync(search, currentPage, pageSize, orderBy, select);

            IEnumerable<object> items;

            if (string.IsNullOrEmpty(select))
            {
                var products = (List<Product>)dynamicItems;
                var productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
                items = productResponses.Cast<object>();
            }

            else
                items = (IEnumerable<object>)dynamicItems;

            return new Paginated<object>
            {
                Items = items,
                TotalCount = totalCount,
                CurrentPage = currentPage,
                PageSize = pageSize,
                //TotalPage đã tính toán trong Model Paginated<T>
            };
        }

        public Product GetById(int id)
        {
            var product = _unitOfWork.Product.Get(p => p.ProductId == id);
            if (product == null)
                throw new NotFoundException($"Product with id #{id} not found");
            return product;
        }

        public async Task<ProductResponse> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Product.GetAsync(p => p.ProductId == id);
            if (product == null)
            {
                //Exception
                throw new NotFoundException($"Product with id #{id} not found");
            }
            return _mapper.Map<ProductResponse>(product);
        }

        public void Update(Product obj)
        {
            _unitOfWork.Product.Update(obj);
        }

        public async Task<ProductResponse> UpdateAsync(int id, ProductRequest request)
        {
            var obj = await _unitOfWork.Product.GetAsync(p => p.ProductId == id);
            if (obj == null)
                throw new NotFoundException($"Product with id {id} not found");

            var duplicated = _unitOfWork.Product.Get(p => p.Name == request.Name && p.ProductId != id);
            if (duplicated != null)
                throw new ExceptionHandler.ValidationException($"Product name '{request.Name}' already exists.");

            _mapper.Map(request, obj);

            _unitOfWork.Product.Update(obj);
            await _unitOfWork.SaveAsync();

            //load lại product để lấy category
            var loadedPro = await _unitOfWork.Product.GetAsync(p => p.ProductId == id);

            return _mapper.Map<ProductResponse>(loadedPro);

        }
    }
}
