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
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        Task<OrderResponse> UpdateOrderStatusAsync(int orderId, OrderUpdateStatusRequest request);
        Task<OrderResponse> PlaceOrderAsync(OrderRequest request);
    }
}
