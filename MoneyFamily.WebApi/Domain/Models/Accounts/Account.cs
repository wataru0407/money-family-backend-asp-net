using MoneyFamily.WebApi.Domain.Models.Users;

namespace MoneyFamily.WebApi.Domain.Models.Accounts
{
    public class Account
    {
        public AccountId Id { get; }
        public AccountName Name { get; private set; }
        public UserId CreateUser { get; }
        private List<UserId> Members { get; }

        private const int MaxMemberCount = 10;

        private Account(AccountId id, AccountName name, UserId createUser, List<UserId> members)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CreateUser = createUser ?? throw new ArgumentNullException(nameof(createUser));
            Members = members ?? throw new ArgumentNullException(nameof(members));
        }

        public static Account CreateNew(AccountName name, UserId createUser)
        {
            var id = new AccountId(Guid.NewGuid());
            var members = new List<UserId>() { createUser };
            return new Account(id, name, createUser, members);
        }

        public static Account CreateFromRepository(AccountId id, AccountName name, UserId createUser, List<UserId> members)
        {
            return new Account(id, name, createUser, members);
        }

        public void Invite(User member)
        {
            if (member is null) throw new ArgumentNullException(nameof(member));
            if (Members.Count >= MaxMemberCount) throw new ArgumentException($"家計簿の人数が上限に達しています。");
            Members.Add(member.Id);
        }

        public void Delete(User member)
        {
            if (member is null) throw new ArgumentNullException(nameof(member));
            Members.Remove(member.Id);
        }

        public List<UserId> GetMembers()
        {
            return Members;
        }

        public void ChangeName(AccountName name)
        {
            Name = name;
        }
    }
}
