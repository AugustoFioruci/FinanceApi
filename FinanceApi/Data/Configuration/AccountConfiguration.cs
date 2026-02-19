using FinanceApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceApi.Data.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder) {
                builder.HasKey(a => a.AccountId);
                builder.Property(a => a.UserId).IsRequired();
                builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
                builder.Property(a => a.Balance).HasColumnType("decimal(18,2)").IsRequired();
                builder.Property(a => a.CreatedDate).HasDefaultValueSql("NOW()");
                builder.HasIndex(a => new { a.UserId, a.Name }).IsUnique();
                builder.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    ;
        }
    }
}
