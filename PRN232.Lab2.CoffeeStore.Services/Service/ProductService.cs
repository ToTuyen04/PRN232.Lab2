using AutoMapper;
using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository;
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

        public Task<ProductResponse> AddAsync(ProductRequest obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product obj)
        {
            _unitOfWork.Product.Remove(obj);
        }

        public Task DeleteAsync(Product obj)
        {
            throw new NotImplementedException();
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
            return _unitOfWork.Product.Get(p => p.ProductId == id);
        }

        public async Task<ProductResponse> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Product.GetAsync(p => p.ProductId == id);
            if (product == null)
            {
                //Exception
                throw new Exception("Product not found");
            }
            return _mapper.Map<ProductResponse>(product);
        }

        public void Update(Product obj)
        {
            _unitOfWork.Product.Update(obj);
        }

        public async Task<ProductResponse> UpdateAsync(int id, ProductRequest obj)
        {
            Product product = _unitOfWork.Product.Get(p => p.ProductId == id);
            if (product == null)
            {
                //Exception
                throw new Exception("Product not found");
            }
            _mapper.Map(obj, product);
            _unitOfWork.Product.Update(product);
            await _unitOfWork.SaveAsync();
            var response = _mapper.Map<ProductResponse>(product);
            return response;

        }
    }
}
