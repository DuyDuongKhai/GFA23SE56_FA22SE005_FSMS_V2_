using FSMS.Entity.Models;
using FSMS.Service.ViewModels.Orders;

namespace FSMS.Service.Services.OrderServices
{
    public interface IOrderService
    {
        Task<List<GetOrder>> GetAllAsync(int? buyerUserId = null, int? sellerUserId = null, string? status = null);
        Task<GetOrder> GetAsync(int key);
        Task CreateOrderAsync(CreateOrder createOrder);
        Task UpdateOrderAsync(int key, UpdateOrder updateOrder);
        Task DeleteOrderAsync(int key);
        Task<Order> ProcessOrderAsync(int orderId, string action);
    }
}
