using MoneyFamily.WebApi.Domain.Models.Users;
using System.Collections.ObjectModel;

namespace MoneyFamily.WebApi.Domain.Models.Accounts
{
    public class Account
    {
        public AccountId Id { get; }
        public AccountName Name { get; private set; }
        public UserId CreateUser { get; }
        private List<UserId> Members { get; }

        private const int MaxMemberCount = 10;

        public Account(AccountId id, AccountName name, UserId createUser, List<UserId> members)
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

        public void AddMember(UserId userId)
        {
            if (userId is null) throw new ArgumentNullException(nameof(userId));
            if (Members.Count >= MaxMemberCount) throw new ArgumentException($"家計簿の人数が上限に達しています。");
            Members.Add(userId);
        }

        public void RemoveMember(UserId userId)
        {
            if (userId is null) throw new ArgumentNullException(nameof(userId));
            if (userId.Value == CreateUser.Value) throw new ArgumentException("家計簿の作成者は削除できません。");
            Members.Remove(userId);
        }

        public ReadOnlyCollection<UserId> GetMembers()
        {
            return Members.AsReadOnly();
        }

        public List<Guid> GetMemberIds()
        {
            return Members.Select(x => x.Value).ToList();
        }

        public void ChangeName(AccountName name)
        {
            Name = name;
        }
    }
}
