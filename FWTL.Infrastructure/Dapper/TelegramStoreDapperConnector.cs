using FWTL.Core.Services.Dapper;
using FWTL.Core.Sql;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace FWTL.Infrastructure
{
    public class TelegramStoreDapperConnector<TCredentials> : IDisposable, IDatabaseConnector<TCredentials> where TCredentials : IDatabaseCredentials
    {
        private readonly TCredentials _credentials;

        public TelegramStoreDapperConnector(TCredentials credentials)
        {
            _credentials = credentials;
        }

        public void Dispose()
        {
        }

        public void Execute(Action<IDbConnection> data)
        {
            using (var databaseConnection = new SqlConnection(_credentials.ConnectionString))
            {
                databaseConnection.Open();
                data(databaseConnection);
            }
        }

        public T Execute<T>(Func<IDbConnection, T> data)
        {
            using (var databaseConnection = new SqlConnection(_credentials.ConnectionString))
            {
                databaseConnection.Open();
                return data(databaseConnection);
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<IDbConnection, Task<T>> data)
        {
            using (var databaseConnection = new SqlConnection(_credentials.ConnectionString))
            {
                await databaseConnection.OpenAsync().ConfigureAwait(false);
                return await data(databaseConnection).ConfigureAwait(false);
            }
        }

        public async Task ExecuteAsync(Func<IDbConnection, Task> data)
        {
            using (var databaseConnection = new SqlConnection(_credentials.ConnectionString))
            {
                await databaseConnection.OpenAsync().ConfigureAwait(false);
                await data(databaseConnection).ConfigureAwait(false);
            }
        }
    }
}