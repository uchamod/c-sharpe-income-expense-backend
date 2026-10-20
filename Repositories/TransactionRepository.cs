using income_expense_tracker.Data;
using income_expense_tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace income_expense_tracker.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {

        private readonly TransactionDbContext _context;

        public TransactionRepository(TransactionDbContext context) => _context = context;

        public async Task<Transaction> AddAsync(Transaction t)
        {
            _context.transactions.Add(t);
            await _context.SaveChangesAsync();
            return t;
        }

        public void DeleteAsync(Transaction t)
        {

            _context.transactions.Remove(t);

        }
        public Task<List<Transaction>> GetAllAsync() =>
        
            _context.transactions.AsNoTracking()
                  .OrderByDescending(x => x.Date)
                  .ThenByDescending(x => x.Id)
                  .ToListAsync();
        

        public Task<Transaction?> GetAllAsyncById(long id) =>
        
           _context.transactions.FirstOrDefaultAsync(x => x.Id == id);

       

        public Task UpdateAsync() =>
        
            _context.SaveChangesAsync();


        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

    }
}
