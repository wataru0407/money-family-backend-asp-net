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
        private readonly IUserFactory userFactory;

        public UserAppricationService(IUserRepository userRepository, UserService userService, IUserFactory userFactory)
        {
            this.userRepository = userRepository;
            this.userService = userService;
            this.userFactory = userFactory;
        }

        public async Task<UserLoginResult> Login(UserLoginCommand command)
        {
            var email = new EmailAddress(command.Email);
            var found = await userRepository.FindByEmail(email);
            if (found == null) throw new CustomNotFoundException($"ユーザが見つかりません。メールアドレス：{command.Email}");

            var password = new Password(command.Password);
            //var hashPassword = userService.CreateHashPassword(email, password);
            //if (hashPassword.Value != found.Password.Value) throw new CustomCanNotLoginException($"パスワードが一致しません。");

            var loginUser = new User(found.Id, found.Name, email, password);
            var isMatchPassword = loginUser.HashPassword == found.HashPassword;
            if (!isMatchPassword) throw new CustomCanNotLoginException("パスワードが一致しません。");

            return new UserLoginResult(found.Id.Value);
        }

        public async Task<UserCreateResult> CreateUser(UserCreateCommand command)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var user = userFactory.CreateNew(
                new UserName(command.Name),
                new EmailAddress(command.Email),
                new Password(command.Password)
                );

            var exists = await userService.Exists(user);
            if (exists) throw new CustomDuplicateException($"同じメールアドレスのユーザがすでに存在しています。メールアドレス：{command.Email}");

            await userRepository.Save(user);

            transaction.Complete();

            return new UserCreateResult(
                user.Id.Value,
                user.Name.Value,
                user.Email.Value
                );
        }

        public async Task<UserGetResult> Get(UserGetCommand command)
        {
            var id = new UserId(command.Id);
            var user = await userRepository.FindById(id);
            if (user == null) throw new CustomNotFoundException($"ユーザが見つかりません。ユーザID：{command.Id}");

            return new UserGetResult(user.Id.Value, user.Name.Value, user.Email.Value);
        }

        public async Task<UserQueryResult> GetQuery(UserQueryCommand command)
        {
            var mail = new EmailAddress(command.Email);
            var user = await userRepository.FindByEmail(mail);
            if (user == null) throw new CustomNotFoundException($"ユーザが見つかりません。メールアドレスID：{command.Email}");

            return new UserQueryResult(user.Id.Value, user.Name.Value, user.Email.Value);
        }



        public async Task<UserUpdateResult> Update(UserUpdateCommand command)
        {
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
                var email = new EmailAddress(command.Email);
                user.ChageEmail(email);

                if (await userService.Exists(user)) throw new CustomDuplicateException($"同じメールアドレスのユーザがすでに存在しています。メールアドレス：{command.Email}");
            }

            if (command.Password is not null)
            {
                var password = new Password(command.Password);
                user.ChangePassword(password);
            }

            await userRepository.Update(user);

            transaction.Complete();

            return new UserUpdateResult(
                command.Id,
                command.Name,
                command.Email
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
