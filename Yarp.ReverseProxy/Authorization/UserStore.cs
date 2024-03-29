namespace Yarp.Gateways.Authorization
{
    public class UserStore
    {
        private static readonly List<User> Users =
        [
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
                Permissions =
                [
                    new UserPermission { UserId = 1, PermissionName = Permissions.User },
                    new UserPermission { UserId = 1, PermissionName = Permissions.Role },
                    new UserPermission { UserId = 1, PermissionName = Permissions.TodoCreate },
                ]
            },

            new User
            {
                Id = 3,
                Name = "bob",
                Password = "111111",
                Role = "user",
                Email = "bob@gmail.com",
                PhoneNumber = "18800000002",
                Permissions =
                [
                    new UserPermission { UserId = 2, PermissionName = Permissions.UserRead },
                    new UserPermission { UserId = 2, PermissionName = Permissions.RoleRead }
                ]
            }

        ];

        public bool CheckPermission(int userId, string permissionName)
        {
            var user = Users.Find(x => x.Id == userId);
            if (user == null) return false;

            return user.Permissions.Any(p => permissionName.AsSpan(1).StartsWith(p.PermissionName,StringComparison.OrdinalIgnoreCase));
        }
    }
}
