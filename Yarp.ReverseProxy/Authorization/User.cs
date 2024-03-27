namespace DaprIdentity.Authorization;

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