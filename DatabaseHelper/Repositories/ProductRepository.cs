using BMB.Data.Models;
using BMB.DatabaseHelper.Interfaces;

namespace BMB.DatabaseHelper.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IAppDatabase _db;

        public ProductRepository(IAppDatabase db)
        {
            _db = db;
        }

        public async Task<int> AddProduct(Product product)
        {
            try
            {
                return await _db.AddAsync(product);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding product", ex);
            }
        }

        public async Task<int> UpdateProduct(Product product)
        {
            try
            {
                return await _db.UpdateAsync(product);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating product", ex);
            }
        }

        public async Task<int> DeleteProduct(Product product)
        {
            try
            {
                return await _db.DeleteAsync(product);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting product", ex);
            }
        }

        public async Task<List<Product>> GetAllProducts()
        {
            try
            {
                return await _db.GetAllAsync<Product>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving products", ex);
            }
        }
    }
}
