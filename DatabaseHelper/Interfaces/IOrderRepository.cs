using BMB.Data.Models;

namespace BMB.DatabaseHelper.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> AddOrder(Order order);
        Task<int> UpdateOrder(Order order);
        Task<int> DeleteOrder(int id);
        Task<List<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
        Task<List<Order>> GetOrdersByClientId(int clientId);
        Task<List<Order>> GetOrdersByUserId(int userId);
        Task<int> AddOrderItem(OrderItem item);
        Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId);
        Task<int> DeleteOrderItemsByOrderId(int orderId);
    }
}
