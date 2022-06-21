namespace MoneyFamily.WebApi.Domain.Models.Users
{
    public interface IUserFactory
    {
        User CreateLogin(UserId id, UserName name, EmailAddress email, Password password);
        User CreateNew(UserName name, EmailAddress email, Password password);
        User CreateFromRepository(UserId id, UserName name, EmailAddress email, string HashPassword);
    }
}
