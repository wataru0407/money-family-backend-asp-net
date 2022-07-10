using MoneyFamily.WebApi.Application.Exceptions;
using MoneyFamily.WebApi.Application.Users.Create;
using MoneyFamily.WebApi.Application.Users.Delete;
using MoneyFamily.WebApi.Application.Users.Get;
using MoneyFamily.WebApi.Application.Users.Login;
using MoneyFamily.WebApi.Application.Users.Query;
using MoneyFamily.WebApi.Application.Users.Update;
using MoneyFamily.WebApi.Domain.Models.Users;
using MoneyFamily.WebApi.Domain.Repository;
using MoneyFamily.WebApi.Domain.Service;
using System.Transactions;

namespace MoneyFamily.WebApi.Application.Users
{
    public class UserApplicationService
    {
        private readonly IUserFactory userFactory;
        private readonly IUserRepository userRepository;
        private readonly UserService userService;

        public UserApplicationService(IUserFactory userFactory, IUserRepository userRepository, UserService userService)
        {
            this.userFactory = userFactory;
            this.userRepository = userRepository;
            this.userService = userService;
        }

        public async Task<UserLoginResult> Login(UserLoginCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var email = new EmailAddress(command.Email);
            var foundUser = await userRepository.FindByEmail(email);
            if (foundUser == null) throw new CustomNotFoundException($"ユーザが見つかりません。メールアドレス：{command.Email}");

            var password = new Password(command.Password);
            var loginUser = userFactory.CreateLogin(foundUser.Id, foundUser.Name, email, password);
            if (loginUser.HashPassword != foundUser.HashPassword) throw new CustomCanNotLoginException("パスワードが一致しません。");

            return new UserLoginResult(foundUser.Id.Value);
        }

        public async Task<UserCreateResult> Create(UserCreateCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = userFactory.CreateNew(
                new UserName(command.Name),
                new EmailAddress(command.Email),
                new Password(command.Password));

            var exists = await userService.Exists(user);
            if (exists) throw new CustomDuplicateException($"同じメールアドレスのユーザがすでに存在しています。メールアドレス：{command.Email}");

            await userRepository.Save(user);
            var result = await userRepository.FindById(user.Id);

            transaction.Complete();

            return new UserCreateResult(
                result.Id.Value,
                result.Name.Value,
                result.Email.Value
                );
        }

        public async Task<UserGetResult> Get(UserGetCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var id = new UserId(command.Id);
            var user = await userRepository.FindById(id);
            if (user == null) throw new CustomNotFoundException($"ユーザが見つかりません。ユーザID：{command.Id}");

            return new UserGetResult(user.Id.Value, user.Name.Value, user.Email.Value);
        }

        public async Task<UserQueryResult> GetQuery(UserQueryCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var mail = new EmailAddress(command.Email);
            var user = await userRepository.FindByEmail(mail);
            if (user == null) throw new CustomNotFoundException($"ユーザが見つかりません。メールアドレスID：{command.Email}");

            return new UserQueryResult(user.Id.Value, user.Name.Value, user.Email.Value);
        }



        public async Task<UserUpdateResult> Update(UserUpdateCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var id = new UserId(command.Id);
            var user = await userRepository.FindById(id);
            if (user == null) throw new CustomNotFoundException($"ユーザが見つかりません。ユーザID：{command.Id}");

            if (command.Name is not null)
            {
                var name = new UserName(command.Name);
                user.ChangeName(name);
            }

            if (command.Email is not null)
            {
                var preEmail = user.Email;
                var email = new EmailAddress(command.Email);
                user.ChageEmail(email);

                if (preEmail != user.Email && await userService.Exists(user)) throw new CustomDuplicateException($"同じメールアドレスのユーザがすでに存在しています。メールアドレス：{command.Email}");
            }

            if (command.Password is not null)
            {
                var password = new Password(command.Password);
                user.ChangePassword(password);
            }

            await userRepository.Update(user);
            var result = await userRepository.FindById(id);

            transaction.Complete();

            return new UserUpdateResult(
                result.Id.Value,
                result.Name.Value,
                result.Email.Value
                );
        }

        public async Task Delete(UserDeleteCommand command)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var id = new UserId(command.Id);
            var user = await userRepository.FindById(id);
            if (user == null) throw new CustomNotFoundException($"ユーザが見つかりません。ユーザID：{command.Id}");

            await userRepository.Delete(user);

            transaction.Complete();
        }
    }
}
