using AI_Wardrobe.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AI_Wardrobe.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderRepo _orderRepo;

        public OrderController(OrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public IActionResult OrderHistory(int userId)
        {

            var orderHistory = _orderRepo.GetOrderHistory(userId);
            foreach (var orderHistoryItem in orderHistory)
            {
                var images = _orderRepo.GetOrderImageUrls(orderHistoryItem.Id);
                orderHistoryItem.Images = images;
            }

            return View(orderHistory);
        }
    }
}
