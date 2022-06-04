using DataAccess.Models;
using DataAccess.SqlDataAccess;

namespace DataAccess.Repositories
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<UserModel>> GetUsers()
        {
            return _db.LoadData<UserModel, dynamic>("dbo.spUser_GetAll", new { });
        }

        public async Task<UserModel> GetUser(int id)
        {
            var results = await _db.LoadData<UserModel, dynamic>("dbo.spUser_Get", new { id = id });
            return results.FirstOrDefault();
        }

        public async Task<UserModel> GetUser(string objectIdentifier)
        {
            var results = 
                await _db.LoadData<UserModel, dynamic>("dbo.spUser_GetByObjIdentifier", new { objectIdentifier = objectIdentifier });
            return results.FirstOrDefault();
        }

        public Task CreateUser(UserModel userModel)
        {
            return _db.SaveData("dbo.spUser_Insert", 
                new { 
                    ObjectIdentifier = userModel.ObjectIdentifier, 
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    DisplayName = userModel.DisplayName,
                    EmailAddress = userModel.EmailAddress
                });
        }

        public Task UpdateUser(UserModel userModel)
        {
            return _db.SaveData("dbo.spUser_Update", userModel);
        }

        public Task DeleteUser(int id)
        {
            return _db.SaveData("dbo.spUser_Delete", new { id = id });
        }
    }
}