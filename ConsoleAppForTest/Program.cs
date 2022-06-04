// See https://aka.ms/new-console-template for more information

using Dapper;
using DataAccess.Models;
using System.Data;
using System.Data.SqlClient;

using IDbConnection cnn = new SqlConnection("Data Source=DESKTOP-A5P1BH0\\SQLEXPRESS;Initial Catalog=StoreDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
try
{
    var u = cnn.Query<UserModel>("dbo.spUser_GetAll", new { }, commandType: CommandType.StoredProcedure);
}
catch (Exception ex)
{
}