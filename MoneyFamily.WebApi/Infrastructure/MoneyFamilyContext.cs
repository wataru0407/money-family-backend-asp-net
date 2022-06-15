using Microsoft.EntityFrameworkCore;
using MoneyFamily.WebApi.Infrastructure.Models;

namespace MoneyFamily.WebApi.Infrastructure
{
    public class MoneyFamilyContext : DbContext
    {
        public MoneyFamilyContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserDto> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDto>(entity =>
            {
                entity.ToTable("users").HasComment("ユーザ");

                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("user_id").HasComment("ユーザID");
                entity.Property(e => e.UserName).HasColumnName("user_name").HasComment("ユーザ名");
                entity.Property(e => e.EmailAddress).HasColumnName("email_address").HasComment("メールアドレス");
                entity.Property(e => e.Password).HasColumnName("password").HasComment("パスワード");
            });
        }
    }
}
