namespace income_expense_tracker.Models
{
    //transaction model class
    public sealed class Transaction
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } =  string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Remark { get; set; }
        public long Date { get; set; }
        public decimal Amount { get; set; }
    }
}
