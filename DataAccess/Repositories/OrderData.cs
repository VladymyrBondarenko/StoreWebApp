using DataAccess.Models;
using DataAccess.SqlDataAccess;

namespace DataAccess.Repositories
{
    public class OrderData : IOrderData
    {
        private readonly ISqlDataAccess _db;

        public OrderData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<OrderModel>> GetAllOrders()
        {
            return _db.LoadMultipleData<OrderModel, UserModel, ProductModel, dynamic>(
                "dbo.spOrders_GetAll",
                new { },
                (order, user, product) =>
                {
                    order.Product = product;
                    order.User = user;
                    return order;
                }, splitOn: "UserId,ProductId");
        }

        public Task<IEnumerable<OrderModel>> GetOrdersByProduct(int productId)
        {
            return _db.LoadMultipleData<OrderModel, UserModel, ProductModel, dynamic>(
                "dbo.spOrders_GetByProduct",
                new { productId = productId },
                (order, user, product) =>
                {
                    order.Product = product;
                    order.User = user;
                    return order;
                }, splitOn: "UserId,ProductId");
        }

        public Task<IEnumerable<OrderModel>> GetOrder(int orderId)
        {
            return _db.LoadMultipleData<OrderModel, UserModel, ProductModel, dynamic>(
                "dbo.spOrders_GetById",
                new { orderId = orderId },
                (order, user, product) =>
                {
                    order.Product = product;
                    order.User = user;
                    return order;
                }, splitOn: "UserId,ProductId");
        }

        public Task<IEnumerable<OrderModel>> GetOrdersByUser(int userId)
        {
            return _db.LoadMultipleData<OrderModel, UserModel, ProductModel, dynamic>(
                "dbo.spOrders_GetByUser",
                new { userId = userId },
                (order, user, product) =>
                {
                    order.Product = product;
                    order.User = user;
                    return order;
                }, splitOn: "UserId,ProductId");
        }

        public Task CreateOrder(OrderModel orderModel)
        {
            return _db.SaveData(
                     "dbo.spOrders_Insert",
                     new
                     {
                         ProductId = orderModel.Product.ProductId,
                         UserId = orderModel.User.UserId,
                         IsApproved = orderModel.IsApproved
                     });
        }

        public Task UpdateOrder(OrderModel orderModel)
        {
            return _db.SaveData(
                "dbo.spOrders_Update",
                orderModel);
        }

        public Task DeleteOrder(OrderModel orderModel)
        {
            return _db.SaveData(
                "dbo.spOrders_Delete",
                new
                {
                    OrderId = orderModel.OrderId
                });
        }
        
        public async Task ApproveOrder(OrderModel orderModel)
        {
            if(orderModel.Product != null)
            {
                try
                {
                    _db.StartTransaction();

                    // set order as Approved
                    await _db.SaveDataInTransaction("dbo.spOrders_Update",
                        new { 
                            OrderId = orderModel.OrderId, 
                            ProductId = orderModel.Product?.ProductId,
                            UserId = orderModel.User?.UserId,
                            IsApproved = true
                        });

                    // set product as unavailable
                    var res = await _db.LoadDataInTransaction<ProductModel, dynamic>(
                        "dbo.spProduct_Get", new { id = orderModel.Product.ProductId });
                    var productByOrder = res.FirstOrDefault();
                    productByOrder.IsAvailable = false;
                    await _db.SaveDataInTransaction("dbo.spProduct_Update", productByOrder);

                    //TODO: set all orders by product as rejected

                    _db.CommitTransaction();
                }
                catch
                {
                    _db.RollbackTransaction();
                    throw;
                }
            }
        }
    }
}