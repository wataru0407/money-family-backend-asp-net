namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public interface IUserFactory
    {
        User CreateNew(UserName name, EmailAddress email, Password password);
        User CreateFromRepository(UserId id, UserName name, EmailAddress email, string HashPassword);
    }
}
