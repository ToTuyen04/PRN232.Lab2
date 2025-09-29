using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Services.RequestModel;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.Service.IService
{
    public interface IOrderService
    {
        Task<OrderResponse> GetOrderByIdAsync(int orderId);
        Task<OrderResponse> UpdateOrderAsync(int orderId, OrderRequest request);
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        Task<Paginated<OrderResponse>> GetAllOrdersAsync(
            string userId,
            string username, 
            string paymentMethod, 
            string select, 
            string orderBy, 
            int currentPage, 
            int pageSize);
        Task<OrderResponse> UpdateOrderStatusAsync(int orderId, OrderUpdateStatusRequest request);
        Task<OrderResponse> PlaceOrderAsync(OrderRequest request);
        Task DeleteAsync(Order obj);

        Order GetById(int id);

    }
}
