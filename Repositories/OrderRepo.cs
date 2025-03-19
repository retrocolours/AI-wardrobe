using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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


        public List<OrderVM> GetOrderHistory(int userId)
        {
            return _aiWardrobeContext.Orders
                .Where(o => o.Fkuserid == userId)
                .OrderByDescending(o => o.Orderid)
                .Select(order => new OrderVM
                {
                    Id = order.Orderid,
                    Date = order.Orderdate,
                    Status = order.Orderstatus,
                }).ToList();
        }

        public List<string> GetOrderImageUrls(int orderId)
        {
            return (from o in _aiWardrobeContext.Orders
                   join od in _aiWardrobeContext.OrderDetails
                   on o.Orderid equals od.Fkorderid
                   join i in _aiWardrobeContext.Items
                   on od.Fkitemid equals i.Itemid
                   where o.Orderid == orderId
                   select i.Imageurl).ToList();
        }

        public OrderVM? GetOrderVM(int orderId)
        {
            var order = _aiWardrobeContext.Orders
                            .Where(order => order.Orderid == orderId).FirstOrDefault();

            if (order != null)
            {
                var orderedBy = _userRepo.GetFullName(order.Fkuserid);
                var deliverAddress = _userRepo.GetFullAddress(order.Fkuserid);

                return new OrderVM
                {
                    Id = orderId,
                    Date = order.Orderdate,
                    Status = order.Orderstatus,
                    OrderedBy = orderedBy,
                    DeliverAddress = deliverAddress,
                };
            }
            else
            {
                return null;
            }
        }

        public OrderDetailVM? GetOrderDetailVMs(int orderId)
        {
            var orderDetail = _aiWardrobeContext.OrderDetails.Where(od => od.Fkorderid == orderId).FirstOrDefault();

            if (orderDetail != null)
            {
                return new OrderDetailVM
                {
                    Id = orderDetail.Orderdetailsid,
                    Quantity = orderDetail.Quantity,
                    Price = orderDetail.Price,
                    productVM = _productRepo.GetProductVm(orderDetail.Fkitemid!)
                };
            }
            else
            {
                return null;
            }
        }

        public bool UpdateOrderStatus(int orderId, string orderStatus)
        {
            try
            {
                var order = GetOrder(orderId);
                if (order != null)
                {
                    order.Orderstatus = orderStatus;
                    _aiWardrobeContext.Orders.Update(order);
                    _aiWardrobeContext.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<SelectListItem> GetStatusOptions()
        {
            List<SelectListItem> statusOptions = new List<SelectListItem>();

            statusOptions.AddRange(_aiWardrobeContext.OrderStatuses.Select(os => new SelectListItem
            {
                Text = os.Status,
                Value = os.Status
            }).ToList());

            return statusOptions;
        }

    }
}
