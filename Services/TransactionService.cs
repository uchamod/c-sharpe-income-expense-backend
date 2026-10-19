using income_expense_tracker.DTO;
using income_expense_tracker.Models;
using income_expense_tracker.Repositories;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace income_expense_tracker.Services
{
    public class TransactionService : ITranscationService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository) => _transactionRepository = transactionRepository;


        //GETALL
        public async Task<List<TransactionResponse>> GetAllAsync() =>
           (await _transactionRepository.GetAllAsync()).Select(ToResponse).ToList();


        //GETALLBYID
        public async Task<TransactionResponse?> GetByIdAsync(long id)
        {
            var item = await _transactionRepository.GetAllAsyncById(id);
            return item is null ? null : ToResponse(item);
        }

        //CREATE
        public async Task<TransactionResponse> CreateAsync(TranscationRequest request)
        {
            var item = new Transaction();
            SetFields(item, request);
            await _transactionRepository.AddAsync(item);
            return ToResponse(item);
        }
                    
        //UPDATE
        public async Task<TransactionResponse?> UpdateAsync(long id, TranscationRequest request)
        {
            var item = await _transactionRepository.GetAllAsyncById(id);
            if (item is null) return null;

            SetFields(item, request);
            await _transactionRepository.UpdateAsync();
            return ToResponse(item);
        }

        //DELETE
        public async Task<bool> DeleteAsync(long id)
        {
            var item = await _transactionRepository.GetAllAsyncById(id);
            if (item is null) return false;
            _transactionRepository.DeleteAsync(item);
            await _transactionRepository.UpdateAsync();
            return true;
        }


//----------------------------------------------------------------------------------------------------//


        //helper methods
        private static TransactionResponse ToResponse(Transaction t) => new(
             t.Id,
             t.Title,
             t.Category,
             t.Amount,
             t.Type,
             t.Date,
             t.Remark
         );

        private static void SetFields(Transaction t,TranscationRequest request)
        {
            t.Title = request.Title.Trim();
            t.Category = request.Category.Trim().ToUpperInvariant();
            t.Amount = request.Amount;
            t.Type = request.Type.Trim().ToUpperInvariant();
            t.Date = request.Date;
            t.Remark = string.IsNullOrWhiteSpace(request.Remark)
                ? null : request.Remark.Trim();

        }
    }

    
}
