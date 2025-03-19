using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;
using System.Text.Json;

namespace AI_Wardrobe.Repositories
{
    public class CookieRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCartCookieKey(string userId)
        {
            return $"ShoppingCart_{userId}";
        }

        public List<CartItem> GetCartItems(string userId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return new List<CartItem>();

            //remove @ sign to prevent error in cookie storage
            string sanitizedUserId = userId.Replace("@", "");
            var cartJson = httpContext.Request.Cookies[GetCartCookieKey(sanitizedUserId)];

            if (string.IsNullOrEmpty(cartJson))
                return new List<CartItem>();

            try
            {
                return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
            }
            catch
            {
                return new List<CartItem>();
            }
        }

        public void AddItem(string userId, int productId, string imageUrl, string productName, decimal price)
        {
            var cart = GetCartItems(userId);
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    UserId = userId,
                    ProductImage = imageUrl,
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = 1
                });
            }

            SaveCart(userId, cart);
        }

        public void RemoveItem(string userId, int productId)
        {
            var cart = GetCartItems(userId);
            var item = cart.FirstOrDefault(c => c.ProductId == productId);

            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
            }

            SaveCart(userId, cart);
        }

        private void SaveCart(string userId, List<CartItem> cart)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return;

            var cartJson = JsonSerializer.Serialize(cart);
            //remove @ sign to prevent error in cookie storage
            string sanitizedUserId = userId.Replace("@", "");
            httpContext.Response.Cookies.Append(GetCartCookieKey(sanitizedUserId), cartJson, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
        }

        public void ClearCartItems(string userId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return;

            // Remove @ sign to prevent error in cookie storage
            string sanitizedUserId = userId.Replace("@", "");
            string cookieKey = GetCartCookieKey(sanitizedUserId);

            // Check if the cookie exists
            if (httpContext.Request.Cookies.ContainsKey(cookieKey))
            {
                // Delete the cookie
                httpContext.Response.Cookies.Delete(cookieKey);
            }
        }
    }
}
