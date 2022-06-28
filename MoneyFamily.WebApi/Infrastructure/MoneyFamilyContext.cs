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
        public DbSet<AccountDto> Accounts { get; set; }
        public DbSet<AccountUserRelationDto> AccountUserRelations { get; set; }

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

            modelBuilder.Entity<AccountDto>(entity =>
            {
                entity.ToTable("account").HasComment("家計簿");

                entity.HasKey(e => e.AccountId);

                entity.Property(e => e.AccountId).HasColumnName("account_id").HasComment("家計簿ID");
                entity.Property(e => e.AccountName).HasColumnName("account_name").HasComment("家計簿名");
                entity.Property(e => e.CreatedUserId).HasColumnName("created_user_id").HasComment("作成ユーザID");
            });

            modelBuilder.Entity<AccountUserRelationDto>(entity =>
            {
                entity.ToTable("account_user_relation").HasComment("ユーザ-家計簿紐づけ");

                entity.HasKey(e => new { e.AccountId, e.UserId });

                entity.Property(e => e.AccountId).HasColumnName("account_id").HasComment("家計簿ID");
                entity.Property(e => e.UserId).HasColumnName("user_id").HasComment("ユーザID");
            });
        }
    }
}
