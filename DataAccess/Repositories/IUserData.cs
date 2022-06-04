using DataAccess.Models;

namespace DataAccess.Repositories
{
    public interface IUserData
    {
        Task CreateUser(UserModel userModel);
        Task DeleteUser(int id);
        Task<UserModel> GetUser(int id);
        Task<UserModel> GetUser(string id);
        Task<IEnumerable<UserModel>> GetUsers();
        Task UpdateUser(UserModel userModel);
    }
}