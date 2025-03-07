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
                    Amount = t.Totalamount,
                    CreateTime = t.Transactiondate.HasValue ? t.Transactiondate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
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

        public bool AddTransaction(TransactionVM transaction, int userId)
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

                var newTransaction = new Transaction
                {
                    Totalamount = transaction.Amount,
                    Transactiondate = transaction.CreateTime.HasValue ? DateOnly.FromDateTime(transaction.CreateTime.Value) : DateOnly.FromDateTime(DateTime.UtcNow),
                    Transactionstatus = transaction.Transactionstatus ?? "Completed",
                    Fkorderid = order.Orderid,
                };

                _context.Transactions.Add(newTransaction);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
