using Data_Integration.Models;

namespace Data_Integration.Services.ProductDetails
{
    public interface IProductServices
    {
        Task<bool> AddProducts(Product productRequest);
    }
}
