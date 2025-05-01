using FinanceAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace FinanceAPI.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .HasOne<User>()
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.Username)
                .HasPrincipalKey(u => u.Username);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Username)
                .HasColumnName("UserId");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
