namespace income_expense_tracker.DTO
{
    //custom rresponse class for transaction
    public sealed record TransactionResponse(
        long Id,
        string Title,
        string Category,
        decimal Amount,
        string Type,
        long Date,
        string? Remark
        );
    

    
}
