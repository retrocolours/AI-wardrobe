using AI_Wardrobe.Data;
using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.Generic;
using System.Linq;

namespace AI_Wardrobe.Repositories
{
    public class TransactionRepo
    {
        private readonly AiwardrobeContext _context;
        private readonly UserRepo _userRepo;
        private readonly CookieRepository _cookieRepo;

        public TransactionRepo(AiwardrobeContext context, UserRepo userRepo, CookieRepository cookieRepo)
        {
            _context = context;
            _userRepo = userRepo;
            _cookieRepo = cookieRepo;
        }

        public IEnumerable<TransactionVM> GetAllTransactions()
        {
            return _context.Transactions?
                .Select(t => new TransactionVM
                {
                    Amount = t.Totalamount,
                    CreateTime = t.Transactiondate.HasValue ? t.Transactiondate.Value : null,
                    Transactionstatus = t.Transactionstatus,
                }) ?? Enumerable.Empty<TransactionVM>();
        }

        public int GetNextTransactionId()
        {
            try
            {
                var lastTransaction = _context.Transactions?.OrderByDescending(t => t.Transactionid).FirstOrDefault();
                return lastTransaction == null ? 1 : lastTransaction.Transactionid + 1;
            }
            catch
            {
                return 1000; // Fallback in case of an error
            }
        }

        public bool AddTransaction(TransactionVM transactionVm, int userId, string email)
        {
            try
            {
                var order = new Order
                {
                    Orderdate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Fkuserid = userId
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

                //add the order details as well
                var cartItems = _cookieRepo.GetCartItems(email);
                foreach (var cartItem in cartItems)
                {
                    var orderDetails = new OrderDetail
                    {
                        Fkitemid = cartItem.ProductId,
                        Fkorderid = order.Orderid,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Price
                    };

                    _context.OrderDetails.Add(orderDetails);
                    _context.SaveChanges();
                }


                var newTransaction = new Transaction
                {
                    Totalamount = transactionVm.Amount,
                    Transactiondate = transactionVm.CreateTime.HasValue ? transactionVm.CreateTime.Value : DateTime.UtcNow,
                    Transactionstatus = transactionVm.Transactionstatus ?? "Complete",
                    Paypaltransactionid = transactionVm.TransactionId,
                    Payername = transactionVm.PayerName,
                    Payeremail = transactionVm.PayerEmail,
                    Currency = transactionVm.Currency,
                    Paymentmethod = transactionVm.PaymentMethod,
                    Fkorderid = order.Orderid,
                };

                _context.Transactions.Add(newTransaction);
                _context.SaveChanges();
                _cookieRepo.ClearCartItems(email);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public TransactionVM? GetTransactionVm(int orderId)
        {
            return _context.Transactions
                .Where(t => t.Fkorderid == orderId)
                .Select(t => new TransactionVM
                {
                    TransactionId = t.Paypaltransactionid,
                    CreateTime = t.Transactiondate,
                    PayerName = t.Payername,
                    PayerEmail = t.Payeremail,
                    Amount = t.Totalamount,
                    Currency = t.Currency,
                    PaymentMethod = t.Paymentmethod,
                    Transactionstatus = t.Transactionstatus
                }).FirstOrDefault();
        }

    }
}
