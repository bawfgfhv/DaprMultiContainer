using static Identity.UserModule.Index;

namespace Identity.UserModule
{
    public interface IBaseService
    {

    }
    public interface IUserService : IBaseService
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        bool AddUser(UserInfo userInfo);
    }

    public class UserService : IUserService
    {
        public static readonly List<UserInfo> UserInfos = new List<UserInfo>();

        public bool AddUser(UserInfo userInfo)
        {
            UserInfos.Add(userInfo);

            Console.WriteLine(UserInfos.Count);
            return true;
        }

        public bool RemoveUser(int id)
        {
            UserInfos.RemoveAll(x => x.Id == id);
            return true;
        }
    }
}
