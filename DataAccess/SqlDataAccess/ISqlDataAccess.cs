
namespace DataAccess.SqlDataAccess
{
    public interface ISqlDataAccess : IDisposable
    {
        Task<IEnumerable<T>> LoadData<T, U>(
            string storedProcedure, U parameters, string connectionId = "Default");

        Task<IEnumerable<T>> LoadMultipleData<T, I, J, U>(
            string storedProcedure, U parameters, Func<T, I, J, T> map, string connectionId = "Default", string splitOn = "");

        Task<IEnumerable<T>> LoadMultipleData<T, I, U>(
            string storedProcedure, U parameters, Func<T, I, T> map, string connectionId = "Default", string splitOn = "");

        Task SaveData<T>(
            string storedProcedure, T parameters, string connectionId = "Default");

        void StartTransaction(string connectionId = "Default");

        Task<IEnumerable<T>> LoadDataInTransaction<T, U>(string storedProcedure, U parameters);

        Task SaveDataInTransaction<T>(string storedProcedure, T parameters);

        void CommitTransaction();

        void RollbackTransaction();
    }
}