using System.ComponentModel.DataAnnotations;

namespace income_expense_tracker.DTO
{
    public class TranscationRequest : IValidatableObject
    {

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [Required]
         public string Category { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "9999999999999999.99")]
        public decimal Amount { get; set; }

        [StringLength(500)]
        public string? Remark { get; set; }


        [Range(typeof(long), "1", "253402300799999")]
        public long Date { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Title))
                yield return new ValidationResult("Title cannot be blank", new[] { nameof(Title) });

            var type = Type?.Trim().ToUpperInvariant();
            var category =Category?.Trim().ToUpperInvariant();
            var validcategory = type switch
            {
                "INCOME" => category is "SALARY" or "PROFIT" or "TIP",
                "EXPENSE" => category is "PERSONAL" or "FOOD" or "TRANSPORT",
                _ => false
            };
            if (!validcategory)
                yield return new ValidationResult(
                    "Type must be INCOME or EXPENSE, with a matching category.",
                    new[] { nameof(Type), nameof(Category) });

            if (Amount != decimal.Round(Amount, 2))
                yield return new ValidationResult("Amount can have at most 2 decimal places.", new[] { nameof(Amount) });

        }
    }
}
