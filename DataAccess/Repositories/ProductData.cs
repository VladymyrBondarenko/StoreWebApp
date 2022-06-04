using DataAccess.Models;
using DataAccess.SqlDataAccess;

namespace DataAccess.Repositories
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _db;

        public ProductData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<ProductModel>> GetProducts()
        {
            return _db.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { });
        }

        public async Task<ProductModel> GetProduct(int id)
        {
            var results = await _db.LoadData<ProductModel, dynamic>("dbo.spProduct_Get", new { id = id });
            return results.FirstOrDefault();
        }

        public Task CreateProduct(ProductModel productModel)
        {
            return _db.SaveData("dbo.spProduct_Insert", 
                new { 
                    ShortDescription = productModel.ShortDescription, 
                    LongDescription = productModel.LongDescription, 
                    Price = productModel.Price, 
                    ImagePath = productModel.ImagePath }
                );
        }

        public Task UpdateProduct(ProductModel productModel)
        {
            return _db.SaveData("dbo.spProduct_Update", productModel);
        }

        public async Task DeleteProduct(int id)
        {
            var p = await GetProduct(id);
            if (File.Exists(p.ImagePath))
            {
                File.Delete(p.ImagePath);
            }
            await _db.SaveData("dbo.spProduct_Delete", new { id = id });
        }
    }
}