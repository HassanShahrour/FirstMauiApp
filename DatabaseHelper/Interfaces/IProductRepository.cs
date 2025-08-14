using BMB.Data.Models;

namespace BMB.DatabaseHelper.Interfaces
{
    public interface IProductRepository
    {
        Task<int> AddProduct(Product product);
        Task<int> UpdateProduct(Product product);
        Task<int> DeleteProduct(Product product);
        Task<List<Product>> GetAllProducts();
    }
}
