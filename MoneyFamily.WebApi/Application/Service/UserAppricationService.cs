using MoneyFamily.WebApi.Application.Exceptions;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using MoneyFamily.WebApi.Domain.Service;
using System.Transactions;

namespace MoneyFamily.WebApi.Application.Service
{
    public class UserAppricationService
    {
        private readonly IUserRepository userRepository;
        private readonly UserService userService;

        public UserAppricationService(IUserRepository userRepository, UserService userService)
        {
            this.userRepository = userRepository;
            this.userService = userService;
        }

        public async Task<UserLoginResult> Login(UserLoginCommand command)
        {
            var email = new EmailAddress(command.Email);
            var found = await userRepository.FindByEmail(email);
            if (found == null) throw new CustomNotFoundException($"ユーザが見つかりません。メールアドレス：{command.Email}");

            var password = new Password(command.Password);
            var hashPassword = userService.CreateHashPassword(email, password);
            if (hashPassword.Value != found.Password.Value) throw new CustomCanNotLoginException($"パスワードが一致しません。");

            return new UserLoginResult(found.Id.Value);
        }

        public async Task<UserCreateResult> CreateUser(UserCreateCommand command)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = User.CreateNew(
                new UserName(command.Name),
                new EmailAddress(command.Email),
                new Password(command.Password)
                );
            
            var exists = await userService.Exists(user);
            if (exists) throw new CustomDuplicateException($"同じメールアドレスのユーザがすでに存在しています。メールアドレス：{command.Email}");

            var hashUser = userService.CreateHashUser(user);
            await userRepository.Save(hashUser);

            transaction.Complete();

            return new UserCreateResult(
                hashUser.Id.Value,
                hashUser.Name.Value,
                hashUser.Email.Value
                );
        }

        public async Task<UserGetResult> Get(UserGetCommand command)
        {
            var id = new UserId(command.Id);
            var user = await userRepository.FindById(id);
            if (user == null) throw new CustomNotFoundException($"ユーザが見つかりません。ユーザID：{command.Id}");

            return new UserGetResult(user.Id.Value, user.Name.Value, user.Email.Value);
        }

        public async Task<UserUpdateResult> Update(UserUpdateCommand command)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var id = new UserId(command.Id);
            var hashUser = userRepository.FindById(id);
            if (hashUser == null) throw new CustomNotFoundException($"ユーザが見つかりません。ユーザID：{command.Id}");

            //var user = User.CreateInstance(
            //    id,
            //    new UserName(command.Name),
            //    new EmailAddress(command.Email)
            //    new Password();

            if (command.Name is not null)
            {
                var name = new UserName(command.Name);
                //hashUser.Change();
            }
            return null;

        }
    }
}
