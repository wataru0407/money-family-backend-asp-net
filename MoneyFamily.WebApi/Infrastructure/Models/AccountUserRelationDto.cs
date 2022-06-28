namespace MoneyFamily.WebApi.Infrastructure.Models
{
    public class AccountUserRelationDto
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
    }
}
