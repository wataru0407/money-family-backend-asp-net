namespace MoneyFamily.WebApi.Infrastructure.Models
{
    public class AccountDto
    {
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public Guid CreatedUserId { get; set; }
    }
}
