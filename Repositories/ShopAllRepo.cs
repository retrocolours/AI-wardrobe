

using AI_Wardrobe.Data;
using AI_Wardrobe.ViewModels;
using AI_Wardrobe.Models;

namespace AI_Wardrobe.Repositories
{
    public class ShopAllRepo
    {
        private readonly AiwardrobeContext _context;

        public ShopAllRepo(AiwardrobeContext context)
        {
            _context = context;
        }

        // Get specific product in the database.
        public ShopAllVM? GetProduct(int id)
        {
            var item = _context.Items?
                .Select(p => new ShopAllVM
                {
                    ID = p.ID,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    Currency = p.Currency,
                    Image = p.Image
                })
                .FirstOrDefault(p => p.ID == id);

            return item;
        }

        // Get all products in the database.
        public IEnumerable<ShopAllVM> GetAllProducts()
        {
            var items = _context.Items?
                .Select(p => new ShopAllVM
                {
                    ID = p.ID,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    Currency = p.Currency,
                    Image = p.Image
                }) ?? Enumerable.Empty<ShopAllVM>();

            return items;
        }
    }
}
