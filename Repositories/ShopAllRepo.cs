

using AI_Wardrobe.Data;
using AI_Wardrobe.ViewModels;
using AI_Wardrobe.Models;
using System.Collections.Generic;

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
            var item = _context.Items
                .Where(i => i.Itemid == id)
                .Select(i=> new ShopAllVM
                {
                    ID = i.Itemid,
                    Type = i.Itemtype!,
                    Description = i.Itemdescription,
                    Price = i.Itemprice,
                    Image = i.Imageurl
                })                            
                .FirstOrDefault();
;

            return item;
        }

        // Get all products in the database.
        public IEnumerable<ShopAllVM> GetAllProducts()
        {
            var items = _context.Items?
                .Select(i => new ShopAllVM
                {
                    ID = i.Itemid,
                   Type = i.Itemtype!,
                    Description = i.Itemdescription,
                    Price = i.Itemprice,
                    Image = i.Imageurl
                }) ?? Enumerable.Empty<ShopAllVM>();
            var list = items.ToList();
            return items!;
        }
    }
}
