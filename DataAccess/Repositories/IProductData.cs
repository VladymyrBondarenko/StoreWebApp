using DataAccess.Models;

namespace DataAccess.Repositories
{
    public interface IProductData
    {
        Task CreateProduct(ProductModel productModel);
        Task DeleteProduct(int id);
        Task<ProductModel> GetProduct(int id);
        Task<IEnumerable<ProductModel>> GetProducts();
        Task UpdateProduct(ProductModel productModel);
    }
}