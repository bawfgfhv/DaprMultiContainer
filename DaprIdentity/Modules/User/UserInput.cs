namespace DaprIdentity.Modules.User
{
    [UserInputFilterValidation]
    public class UserInput
    {
        public string UserName { get; set; }
        public string UserMobile { get; set; }
    }
}
