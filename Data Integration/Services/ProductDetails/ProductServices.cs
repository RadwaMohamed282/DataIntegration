using Data_Integration.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Integration.Services.ProductDetails
{
    public class ProductServices : IProductServices
    {

        private readonly ApplicationDbContext _context;
        public ProductServices(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<bool> AddProducts (Product productData)
        {
            if (productData is not null)
            {
                #region Mapping from productData to productData Entity
                var productDetails= new Product()
                {
                   Id= productData.Id,
                   Name= productData.Name,
                   Price= productData.Price
                };
                #endregion
                await _context.Product.AddAsync(productDetails);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
