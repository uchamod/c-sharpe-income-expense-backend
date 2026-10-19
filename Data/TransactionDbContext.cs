using income_expense_tracker.Models;
using Microsoft.EntityFrameworkCore;


namespace income_expense_tracker.Data
{
    public sealed class TransactionDbContext : DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options) { }

        public DbSet<Transaction> transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transactions");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(x => x.Title).HasColumnName("title").HasMaxLength(100).IsRequired();
                entity.Property(x => x.Category).HasColumnName("category").HasMaxLength(20).IsRequired();
                entity.Property(x => x.Amount).HasColumnName("amount").HasPrecision(18, 2);
                entity.Property(x => x.Type).HasColumnName("type").HasMaxLength(10).IsRequired();
                entity.Property(x => x.Date).HasColumnName("date");
                entity.Property(x => x.Remark).HasColumnName("remarks").HasMaxLength(500);
                entity.HasIndex(x => x.Date);

            }
            );
        }

    }
}
