using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;

namespace BMB.DatabaseHelper.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IAppDatabase _db;

        public OrderRepository(IAppDatabase db)
        {
            _db = db;
        }

        public async Task<int> AddOrder(Order order)
        {
            try
            {
                return await _db.AddAsync(order);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding order", ex);
            }
        }

        public async Task<int> UpdateOrder(Order order)
        {
            try
            {
                return await _db.UpdateAsync(order);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating order", ex);
            }
        }

        public async Task<int> DeleteOrder(int id)
        {
            try
            {
                var order = await _db.GetByIdAsync<Order>(id);
                if (order == null)
                    return 0;

                return await _db.DeleteAsync(order);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deleting order with ID {id}", ex);
            }
        }

        public async Task<List<Order>> GetAllOrders()
        {
            try
            {
                return await _db.GetAllAsync<Order>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving all orders", ex);
            }
        }

        public async Task<Order> GetOrderById(int id)
        {
            try
            {
                return await _db.GetByIdAsync<Order>(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving order with ID {id}", ex);
            }
        }

        public async Task<List<Order>> GetOrdersByClientId(int clientId)
        {
            try
            {
                return await _db.GetManyByConditionAsync<Order>(o => o.ClientId == clientId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving orders for client ID {clientId}", ex);
            }
        }

        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            try
            {
                return await _db.GetManyByConditionAsync<Order>(o => o.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving orders for user ID {userId}", ex);
            }
        }

        public async Task<int> AddOrderItem(OrderItem item)
        {
            try
            {
                return await _db.AddAsync(item);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding order item", ex);
            }
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId)
        {
            try
            {
                return await _db.GetManyByConditionAsync<OrderItem>(oi => oi.OrderId == orderId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving order items for order ID {orderId}", ex);
            }
        }

        public async Task<int> DeleteOrderItemsByOrderId(int orderId)
        {
            try
            {
                var items = await GetOrderItemsByOrderId(orderId);
                if (items == null || !items.Any())
                    return 0;

                int deletedCount = 0;
                foreach (var item in items)
                {
                    deletedCount += await _db.DeleteAsync(item);
                }

                return deletedCount;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deleting order items for order ID {orderId}", ex);
            }
        }
    }
}
