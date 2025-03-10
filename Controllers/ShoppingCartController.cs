using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AI_Wardrobe.Repositories;
using AI_Wardrobe.Models;
using System.Collections.Generic;
using System.Text.Json;

[Route("shopping-cart")]
public class ShoppingCartController : Controller
{
    private readonly CookieRepository _cookieRepo;
    private const string SessionCartKey = "GuestCart";

    public ShoppingCartController(CookieRepository cookieRepo)
    {
        _cookieRepo = cookieRepo;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var cartItems = User.Identity.IsAuthenticated
            ? _cookieRepo.GetCartItems(User.Identity.Name)
            : GetGuestCartFromSession();

        return View(cartItems);
    }

    [HttpPost("add")]
    public IActionResult AddItem([FromBody] CartItem item)
    {
        if (item == null || item.ProductId <= 0 || string.IsNullOrEmpty(item.ProductName))
        {
            return BadRequest(new { message = "Invalid product data." });
        }

        if (User.Identity.IsAuthenticated)
        {
            string userName = User.Identity.Name;
            _cookieRepo.AddItem(userName ,item.ProductId, item.ProductImage, item.ProductName, item.Price);
            return Ok(new { message = $"Item '{item.ProductName}' added to cart.", cart = _cookieRepo.GetCartItems(userName) });
        }

        var guestCart = GetGuestCartFromSession();
        var existingItem = guestCart.Find(c => c.ProductId == item.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            guestCart.Add(new CartItem
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Price = item.Price,
                Quantity = 1
            });
        }

        SaveGuestCartToSession(guestCart);
        return Ok(new { message = $"Item '{item.ProductName}' added to guest cart.", cart = guestCart });
    }

    [HttpPost("remove")]
    public IActionResult RemoveItem([FromBody] RemoveItemRequest request)
    {
        if (request == null || request.ProductId <= 0)
        {
            return BadRequest(new { message = "Invalid product ID." });
        }

        if (User.Identity.IsAuthenticated)
        {
            string userName = User.Identity.Name;
            _cookieRepo.RemoveItem(userName, request.ProductId);
            return Ok(new { message = "Item removed from cart.", cart = _cookieRepo.GetCartItems(userName) });
        }

        var guestCart = GetGuestCartFromSession();
        var item = guestCart.Find(c => c.ProductId == request.ProductId);

        if (item != null)
        {
            item.Quantity--;
            if (item.Quantity <= 0)
            {
                guestCart.Remove(item);
            }
        }

        SaveGuestCartToSession(guestCart);
        return Ok(new { message = "Item removed from guest cart.", cart = guestCart });
    }

    [HttpGet("get")]
    public IActionResult GetCart()
    {
        return Ok(User.Identity.IsAuthenticated ? _cookieRepo.GetCartItems(User.Identity.Name) : GetGuestCartFromSession());
    }

    private List<CartItem> GetGuestCartFromSession()
    {
        var cartJson = HttpContext.Session.GetString(SessionCartKey);
        return string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
    }

    private void SaveGuestCartToSession(List<CartItem> cart)
    {
        HttpContext.Session.SetString(SessionCartKey, JsonSerializer.Serialize(cart));
    }
}

public class RemoveItemRequest
{
    public int ProductId { get; set; }
}
