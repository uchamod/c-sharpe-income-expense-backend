using income_expense_tracker.Models;

namespace income_expense_tracker.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction?> GetAllAsyncById(long id);
        Task<Transaction> AddAsync(Transaction t);
        Task UpdateAsync();
        void DeleteAsync(Transaction t);

        Task<int> SaveChangesAsync();
    }
}
