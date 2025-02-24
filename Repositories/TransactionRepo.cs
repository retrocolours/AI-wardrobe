using AI_Wardrobe.Data;
using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AI_Wardrobe.Repositories
{
    public class TransactionRepo
    {
        private readonly AiwardrobeContext _context;
        private readonly UserRepo _userRepo;

        public TransactionRepo(AiwardrobeContext context, UserRepo userRepo)
        {
            _context = context;
            _userRepo = userRepo;
        }


        public IEnumerable<TransactionVM> GetAllTransactions()
        {
            return _context.Transactions?
                .Select(t => new TransactionVM
                {
                    // TransactionId = t.PayPalTransactionId,
                    // PayerName = t.PayerName,
                    // PayerEmail = t.PayerEmail,
                    Amount = t.Totalamount, 
                    CreateTime = t.Transactiondate, 
                    Transactionstatus = t.Transactionstatus,
                    // PaymentMethod = t.PaymentMethod,
                    // Currency = t.Currency
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


        public bool AddTransaction(TransactionVM transaction)
        {
            try
            {

                //FIND LOGGED IN RegisterdUser USER ID 
                var order = new Order{
                    Orderstatus = "Pending",
                    Orderdate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Fkuserid = 1 // users id 
                };
                // CALL ORDER REPO TO ADD ORDER

                _context.Orders.Add(order);
                _context.SaveChanges();

    
                var newTransaction = new Transaction
                {
                    // Transactionid = GetNextTransactionId(),
                    // PayPalTransactionId = transaction.TransactionId, 
                    Totalamount = transaction.Amount, 
                    Transactiondate = transaction.CreateTime ?? DateTime.UtcNow, 
                    Transactionstatus = transaction.Transactionstatus ?? "Completed",
                    Fkorderid = order.Orderid,

                };

                _context.Transactions.Add(newTransaction);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                
                
                return false;
            }
        }
    }
}
