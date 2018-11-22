using FWT.Core.Services.Dapper;
using FWT.Core.Sql;
using FWT.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FWT.Infrastructure.Dapper
{
    public class DapperConnector<TCredentials> : IDatabaseConnector where TCredentials : IDatabaseCredentials
    {
        private readonly IDatabaseCredentials _databaseConnection;

        public DapperConnector(TCredentials databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public void Execute(Action<IDbConnection> data)
        {
            using (var connection = new SqlConnection(_databaseConnection.ConnectionString))
            {
                connection.Open();
                data(connection);
            }
        }

        public T Execute<T>(Func<IDbConnection, T> data)
        {
            using (var connection = new SqlConnection(_databaseConnection.ConnectionString))
            {
                connection.Open();
                return data(connection);
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<IDbConnection, Task<T>> data)
        {
            using (var connection = new SqlConnection(_databaseConnection.ConnectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                return await data(connection).ConfigureAwait(false);
            }
        }

        public async Task ExecuteAsync(Func<IDbConnection, Task> data)
        {
            using (var connection = new SqlConnection(_databaseConnection.ConnectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                await data(connection).ConfigureAwait(false);
            }
        }
    }
}