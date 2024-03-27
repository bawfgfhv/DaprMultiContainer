namespace DaprIdentity.Authorization;

static class Permissions
{
    public const string User = "User";
    public const string UserCreate = "User.Create";
    public const string UserRead = "User.Read";
    public const string UserUpdate = "User.Update";
    public const string UserDelete = "User.Delete";

    public const string Role = "Role";
    public const string RoleRead = "Role.Read";

    public const string Todo = "Todo";
    public const string TodoCreate = "Todo/Create";
}

public class Apis
{
    public static Api UserCreate = new Api("/User/Created", Permissions.UserCreate);
}

public record Api(string Path,string Permission);