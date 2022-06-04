using DataAccess.Models;

namespace DataAccess.Repositories
{
    public interface IOrderData
    {
        Task CreateOrder(OrderModel orderModel);
        Task DeleteOrder(OrderModel orderModel);
        Task<IEnumerable<OrderModel>> GetAllOrders();
        Task<IEnumerable<OrderModel>> GetOrder(int orderId);
        Task<IEnumerable<OrderModel>> GetOrdersByProduct(int productId);
        Task<IEnumerable<OrderModel>> GetOrdersByUser(int orderId);
        Task UpdateOrder(OrderModel orderModel);
        Task ApproveOrder(OrderModel orderModel);
    }
}