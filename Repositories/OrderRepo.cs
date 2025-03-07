using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Drawing;

namespace AI_Wardrobe.Repositories
{
    public class OrderRepo
    {
        private readonly AiwardrobeContext _aiWardrobeContext;
        private readonly ProductRepo _productRepo;
        private readonly UserRepo _userRepo;

        public OrderRepo(AiwardrobeContext aiWardrobeContext, ProductRepo productRepo, UserRepo userRepo)
        {
            _aiWardrobeContext = aiWardrobeContext;
            _productRepo = productRepo;
            _userRepo = userRepo;
        }

        public int? AddOrder(Order order)
        {
            try
            {
                _aiWardrobeContext.Orders.Add(order);
                _aiWardrobeContext.SaveChanges();
                return order.Orderid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding order:" + $" {ex.Message}");
                return null;
            }
        }

        public bool EditOrderStatus(int id, string status)
        {
            bool isSuccess = false;

            var order = GetOrder(id);
            if (order != null)
            {
                order.Orderstatus = status;
                return UpdateOrder(order);
            }

            return isSuccess;
        }

        public bool UpdateOrder(Order order)
        {
            bool isSuccess = false;

            try
            {
                _aiWardrobeContext.Orders.Update(order);
                _aiWardrobeContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order:" + $" {ex.Message}");
                isSuccess = false;
            }

            return isSuccess;
        }

        public Order? GetOrder(int id)
        {
            return _aiWardrobeContext.Orders.Where(o => o.Orderid == id).FirstOrDefault();
        }

        public IEnumerable<Order>? GetOrdersByUser(int userId)
        {
            return _aiWardrobeContext.Orders.Where(o => o.Fkuserid == userId);
        }

        public IEnumerable<Order>? GetAllOrder()
        {
            return _aiWardrobeContext.Orders;
        }

        public bool AddOrderDetail(OrderDetail orderDetail)
        {
            bool isSuccess = false;

            try
            {
                _aiWardrobeContext.OrderDetails.Add(orderDetail);
                _aiWardrobeContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding order detail:" + $" {ex.Message}");
                isSuccess = false;
            }

            return isSuccess;
        }

        public IEnumerable<OrderDetail>? GetOrderDetails(int orderId)
        {
            return _aiWardrobeContext.OrderDetails.Where(od => od.Fkorderid == orderId);
        }


        public OrderVM? GetOrderVM(int orderId)
        {
            var order = _aiWardrobeContext.Orders
                            .Where(order => order.Orderid == orderId).FirstOrDefault();

            if (order != null)
            {
                var orderedBy = _userRepo.GetFullName(order.Orderid);
                var deliverAddress = _userRepo.GetFullAddress(order.Orderid);

                return new OrderVM
                {
                    Id = orderId,
                    Date = order.Orderdate,
                    Status = order.Orderstatus,
                    OrderedBy = _userRepo.GetFullName(order.Orderid),
                    DeliverAddress = _userRepo.GetFullAddress(order.Orderid),
                };
            } else
            {
                return null;
            }
        }

        public IEnumerable<OrderDetailVM> GetOrderDetailVMs(int orderId)
        {
            return _aiWardrobeContext.OrderDetails.Where(od => od.Fkorderid == orderId).Select(od => new OrderDetailVM
            {
                Id = od.Orderdetailsid,
                Quantity = od.Quantity,
                Price = od.Price,
                productVM = _productRepo.GetProductVm(od.Fkitemid)
            });

        }
    }
}
