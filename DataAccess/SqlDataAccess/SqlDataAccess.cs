using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.SqlDataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> LoadMultipleData<T, I, J, U>(
            string storedProcedure, U parameters, Func<T, I, J, T> map, string connectionId = "Default", string splitOn = "")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryAsync(
                storedProcedure, map, param: parameters, commandType: CommandType.StoredProcedure, splitOn: splitOn);
        }

        public async Task<IEnumerable<T>> LoadMultipleData<T, I, U>(
            string storedProcedure, U parameters, Func<T, I, T> map, string connectionId = "Default", string splitOn = "")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryAsync(storedProcedure, map, param: parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "Default")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public void StartTransaction(string connectionId = "Default")
        {
            _connection = new SqlConnection(_config.GetConnectionString(connectionId));
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public async Task<IEnumerable<T>> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            return await _connection.QueryAsync<T>(
                storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public async Task SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
            await _connection.ExecuteAsync(
                storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public void CommitTransaction()
        {
            if(_connection.State == ConnectionState.Open)
            {
                _transaction?.Commit();
                _connection?.Close();
            }
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
        }

        public void Dispose()
        {
            CommitTransaction();
        }
    }
}