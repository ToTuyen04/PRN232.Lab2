using AutoMapper;
using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Repositories.Enums;
using PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository;
using PRN232.Lab2.CoffeeStore.Services.ExceptionHandler;
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
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderResponse> GetOrderByIdAsync(int orderId)
        {
            var order = await _unitOfWork.Order.GetAsync(o => o.OrderId == orderId);
            if (order == null)
                throw new NotFoundException($"Order with id {orderId} not found");
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Order.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<OrderResponse> PlaceOrderAsync(OrderRequest request)
        {
            if (await _unitOfWork.User.GetAsync(u => u.UserId == request.UserId) == null)
                throw new NotFoundException($"User with id {request.UserId} not found");
            if (!Enum.TryParse<PaymentMethod>(request.PaymentMethod, true, out var method)
                || !Enum.IsDefined(typeof(PaymentMethod), method))
                throw new BadRequestException($"Payment method {request.PaymentMethod} is not valid");
            foreach (var item in request.Items)
            {
                if (await _unitOfWork.Product.GetAsync(p => p.ProductId == item.ProductId) == null)
                    throw new NotFoundException($"Product with id {item.ProductId} not found");
                if (item.Quantity <= 0)
                    throw new BadRequestException($"Quantity must be greater than 0");
            }
            //var order = new Order
            //{
            //    //OrderId, => tự tăng
            //    OrderDate = DateTime.Now,
            //    Status = OrderStatus.PENDING.ToString(),
            //    UserId = request.UserId,
            //    OrderDetails = request.Items.Select(i => new OrderDetail
            //    {
            //        ProductId = i.ProductId,
            //        Quantity = i.Quantity,
            //        UnitPrice = _unitOfWork.Product.Get(p => p.ProductId == i.ProductId).Price
            //    }).ToList()
            //};
            var order = _mapper.Map<Order>(request);
            order.OrderDate = DateTime.Now;
            order.Status = OrderStatus.PENDING.ToString();
            // Gán UnitPrice cho từng OrderDetail
            foreach (var od in order.OrderDetails)
            {
                var product = await _unitOfWork.Product.GetAsync(p => p.ProductId == od.ProductId);
                od.UnitPrice = product.Price;
            }
            // Tính tổng số tiền từ chi tiết đơn hàng
            var totalAmount = order.OrderDetails.Sum(od => od.Quantity * od.UnitPrice);
            var payment = new Payment
            {
                PaymentMethod = method.ToString(),
                PaymentDate = DateTime.Now,
                Amount = totalAmount,
            };
            payment.Order = order;

            await _unitOfWork.Order.AddAsync(order);
            await _unitOfWork.Payment.AddAsync(payment);
            await _unitOfWork.SaveAsync();

            //lưu thông tin của payment vào Order
            order.PaymentID = payment.PaymentId;
            _unitOfWork.Order.Update(order);
            await _unitOfWork.SaveAsync();

            //return new OrderResponse
            //{
            //    OrderId = order.OrderId,
            //    OrderDate = order.OrderDate,
            //    Status = order.Status,
            //    UserId = order.UserId,
            //    UserResponse = new UserResponse
            //    {
            //        UserId = order.User.UserId,
            //        UserName = order.User.UserName,
            //        Email = order.User.Email,
            //        Role = order.User.Role
            //    },
            //    PaymentResponse = new PaymentResponse
            //    {
            //        PaymentId = payment.PaymentId,
            //        PaymentMethod = payment.PaymentMethod,
            //        PaymentDate = payment.PaymentDate,
            //        Amount = payment.Amount
            //    },
            //    OrderDetailResponses = order.OrderDetails.Select(od => new OrderDetailResponse
            //    {
            //        OrderDetailId = od.OrderDetailId,
            //        ProductId = od.ProductId,
            //        ProductName = od.Product.Name,
            //        Quantity = od.Quantity,
            //        UnitPrice = od.UnitPrice
            //    }).ToList()
            //};

            var loadedOrder = await _unitOfWork.Order.GetAsync(o => o.OrderId == order.OrderId);
            return _mapper.Map<OrderResponse>(loadedOrder);
        }
        public async Task<OrderResponse> UpdateOrderStatusAsync(int orderId, OrderUpdateStatusRequest request)
        {
            var order = await _unitOfWork.Order.GetAsync(o => o.OrderId == orderId);
            if (order == null)
                throw new NotFoundException($"Order with id {orderId} not found");
            if (!Enum.TryParse<OrderStatus>(request.Status, true, out var newStatus)
                || !Enum.IsDefined(typeof(OrderStatus), newStatus))
                throw new BadRequestException($"Order status {request.Status} is not valid");
            order.Status = newStatus.ToString();
            _unitOfWork.Order.Update(order);
            await _unitOfWork.SaveAsync();
            var updatedOrder = await _unitOfWork.Order.GetAsync(o => o.OrderId == orderId);
            return _mapper.Map<OrderResponse>(updatedOrder);
        }

        // Giữ nguyên return type là Paginated<OrderResponse>
        public async Task<Paginated<OrderResponse>> GetAllOrdersAsync(
            string username,
            string paymentMethod,
            string select,
            string orderBy,
            int currentPage, int pageSize)
        {
            var orders = await _unitOfWork.Order.GetAllAsync();

            var filteredOrders = string.IsNullOrEmpty(username)
                ? orders
                : orders.Where(o => o.User.UserName.Contains(username, StringComparison.OrdinalIgnoreCase));

            filteredOrders = string.IsNullOrEmpty(paymentMethod)
                ? filteredOrders
                : filteredOrders.Where(o => o.Payment.PaymentMethod.Equals(paymentMethod, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(orderBy))
            {
                filteredOrders = ApplyOrdering(filteredOrders, orderBy);
            }

            var pagedOrders = filteredOrders
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            var mappedOrders = _mapper.Map<IEnumerable<OrderResponse>>(pagedOrders);

            // Set SelectedFields cho từng OrderResponse
            if (!string.IsNullOrEmpty(select))
            {
                var selectedFields = select.Split(',').Select(f => f.Trim().ToLower()).ToHashSet();
                foreach (var order in mappedOrders)
                {
                    order.SelectedFields = selectedFields;
                }
            }

            return new Paginated<OrderResponse>
            {
                Items = mappedOrders,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = filteredOrders.Count()
            };
        }

        private IEnumerable<Order> ApplyOrdering(IEnumerable<Order> orders, string orderBy)
        {
            var orderParams = orderBy.Split(',');
            IOrderedEnumerable<Order> orderedOrders = null;

            foreach (var param in orderParams)
            {
                var trimmedParam = param.Trim();
                var isDescending = trimmedParam.StartsWith("-");
                var propertyName = isDescending ? trimmedParam.Substring(1) : trimmedParam;

                switch (propertyName.ToLower())
                {
                    case "orderid":
                        orderedOrders = orderedOrders == null
                            ? (isDescending ? orders.OrderByDescending(o => o.OrderId) : orders.OrderBy(o => o.OrderId))
                            : (isDescending ? orderedOrders.ThenByDescending(o => o.OrderId) : orderedOrders.ThenBy(o => o.OrderId));
                        break;
                    case "orderdate":
                        orderedOrders = orderedOrders == null
                            ? (isDescending ? orders.OrderByDescending(o => o.OrderDate) : orders.OrderBy(o => o.OrderDate))
                            : (isDescending ? orderedOrders.ThenByDescending(o => o.OrderDate) : orderedOrders.ThenBy(o => o.OrderDate));
                        break;
                    case "status":
                        orderedOrders = orderedOrders == null
                            ? (isDescending ? orders.OrderByDescending(o => o.Status) : orders.OrderBy(o => o.Status))
                            : (isDescending ? orderedOrders.ThenByDescending(o => o.Status) : orderedOrders.ThenBy(o => o.Status));
                        break;
                    case "username":
                        orderedOrders = orderedOrders == null
                            ? (isDescending ? orders.OrderByDescending(o => o.User.UserName) : orders.OrderBy(o => o.User.UserName))
                            : (isDescending ? orderedOrders.ThenByDescending(o => o.User.UserName) : orderedOrders.ThenBy(o => o.User.UserName));
                        break;
                    case "paymentmethod":
                        orderedOrders = orderedOrders == null
                            ? (isDescending ? orders.OrderByDescending(o => o.Payment.PaymentMethod) : orders.OrderBy(o => o.Payment.PaymentMethod))
                            : (isDescending ? orderedOrders.ThenByDescending(o => o.Payment.PaymentMethod) : orderedOrders.ThenBy(o => o.Payment.PaymentMethod));
                        break;
                    case "amount":
                        orderedOrders = orderedOrders == null
                            ? (isDescending ? orders.OrderByDescending(o => o.Payment.Amount) : orders.OrderBy(o => o.Payment.Amount))
                            : (isDescending ? orderedOrders.ThenByDescending(o => o.Payment.Amount) : orderedOrders.ThenBy(o => o.Payment.Amount));
                        break;
                    default:
                        // If property not found, skip this ordering
                        continue;
                }
            }

            return orderedOrders ?? orders;
        }

        // Helper method for field selection
        private IEnumerable<OrderResponse> ApplySelection(IEnumerable<OrderResponse> orders, string select)
        {
            var selectedFields = select.Split(',').Select(f => f.Trim().ToLower()).ToList();

            // If no valid fields specified, return all
            if (!selectedFields.Any())
                return orders;

            return orders.Select(order => new OrderResponse
            {
                OrderId = selectedFields.Contains("orderid") ? order.OrderId : 0,
                OrderDate = selectedFields.Contains("orderdate") ? order.OrderDate : DateTime.MinValue,
                Status = selectedFields.Contains("status") ? order.Status : null,
                UserId = selectedFields.Contains("userid") ? order.UserId : null,
                UserResponse = selectedFields.Contains("user") || selectedFields.Contains("userresponse") ? order.UserResponse : null,
                PaymentResponse = selectedFields.Contains("payment") || selectedFields.Contains("paymentresponse") ? order.PaymentResponse : null,
                OrderDetailResponses = selectedFields.Contains("orderdetails") || selectedFields.Contains("orderdetailresponses") ? order.OrderDetailResponses : null
            });
        }
    }
}
