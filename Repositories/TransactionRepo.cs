using AI_Wardrobe.Data;
using AI_Wardrobe.Models;
using AI_Wardrobe.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace AI_Wardrobe.Repositories
{
    public class TransactionRepo
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public IEnumerable<TransactionVM> GetAllTransactions()
        {
            return _context.Transactions?
                .Select(t => new TransactionVM
                {
                    TransactionId = t.PayPalTransactionId,
                    PayerName = t.PayerName,
                    PayerEmail = t.PayerEmail,
                    Amount = t.Totalamount, 
                    CreateTime = t.Transactiondate, 
                    Transactionstatus = t.Transactionstatus,
                    PaymentMethod = t.PaymentMethod,
                    Currency = t.Currency
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
                var newTransaction = new Transaction
                {
                    Transactionid = GetNextTransactionId(),
                    PayPalTransactionId = transaction.TransactionId, 
                    PayerName = transaction.PayerName,
                    PayerEmail = transaction.PayerEmail,
                    Totalamount = transaction.Amount, 
                    Transactiondate = transaction.CreateTime ?? DateTime.UtcNow, 
                    Transactionstatus = transaction.Transactionstatus ?? "Completed",
                    PaymentMethod = transaction.PaymentMethod,
                    Currency = transaction.Currency
                };

                _context.Transactions!.Add(newTransaction);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                
                
                return false;
            }
        }
    }
}
