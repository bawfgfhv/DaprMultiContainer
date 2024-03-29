namespace Yarp.Gateways.Authorization;

internal record UserPermission
{
    public int UserId { get; set; }
    public string PermissionName { get; set; }
}