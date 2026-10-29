using income_expense_tracker.DTO;
//transcation service
namespace income_expense_tracker.Services
{
    public interface ITranscationService
    {
        Task<List<TransactionResponse>> GetAllAsync();
        Task<TransactionResponse?> GetByIdAsync(long id);
        Task<TransactionResponse> CreateAsync(TranscationRequest request);
        Task<TransactionResponse?> UpdateAsync(long id, TranscationRequest request);
        Task<bool> DeleteAsync(long id);

    }
}
