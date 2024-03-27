namespace DaprIdentity.Authorization;

internal record UserPermission
{
    public int UserId { get; set; }
    public string PermissionName { get; set; }
}