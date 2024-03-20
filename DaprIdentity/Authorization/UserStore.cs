using OpenIddict.Abstractions;

namespace DaprIdentity.Authorization
{
    public class UserStore
    {
        private static readonly List<User> _users = new List<User>()
        {
            new User
            {
                Id = 1,
                Name = "admin",
                Password = "111111",
                Role = "admin",
                Email = "admin@gmail.com",
                PhoneNumber = "18800000000"
            },
            new User
            {
                Id = 2,
                Name = "alice",
                Password = "111111",
                Role = "user",
                Email = "alice@gmail.com",
                PhoneNumber = "18800000001",
                Permissions = new List<UserPermission>
                {
                    new UserPermission { UserId = 1, PermissionName = Permissions.User },
                    new UserPermission { UserId = 1, PermissionName = Permissions.Role }
                }
            },
            new User
            {
                Id = 3,
                Name = "bob",
                Password = "111111",
                Role = "user",
                Email = "bob@gmail.com",
                PhoneNumber = "18800000002",
                Permissions = new List<UserPermission>
                {
                    new UserPermission { UserId = 2, PermissionName = Permissions.UserRead },
                    new UserPermission { UserId = 2, PermissionName = Permissions.RoleRead }
                }
            },
        };

        public bool CheckPermission(int userId, string permissionName)
        {
            var user = _users.Find(x => x.Id == userId);
            if (user == null) return false;

            return user.Permissions.Any(p => permissionName.AsSpan(1).StartsWith(p.PermissionName));
        }
    }

    static class Permissions
    {
        public const string User = "User";
        public const string UserCreate = "User.Create";
        public const string UserRead = "User.Read";
        public const string UserUpdate = "User.Update";
        public const string UserDelete = "User.Delete";

        public const string Role = "Role";
        public const string RoleRead = "Role.Read";
    }

    internal record UserPermission
    {
        public int UserId { get; set; }
        public string PermissionName { get; set; }
    }

    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<UserPermission> Permissions { get; set; }
    }


}
