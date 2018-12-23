using System.Threading.Tasks;
using Dapper;
using FWTL.Core.Services.Dapper;
using FWTL.Database;
using OpenTl.ClientApi;
using static FWTL.Core.Entities.Maps.TelegramSessionMap;

namespace FWTL.Infrastructure.Telegram
{
    public class DatabaseSessionStore : ISessionStore
    {
        private readonly IDatabaseConnector<TelegramDatabaseCredentials> _database;

        private string _userId;

        public DatabaseSessionStore(IDatabaseConnector<TelegramDatabaseCredentials> database)
        {
            _database = database;
        }

        public void Dispose()
        {
        }

        public byte[] Load()
        {
            return _database.Execute(conn =>
            {
                return conn.QueryFirstOrDefault<byte[]>($"SELECT {Session} FROM {TelegramSessionTable} WHERE {UserId} = @{UserId}", new { UserId = _userId });
            });
        }

        public async Task Save(byte[] session)
        {
            await _database.ExecuteAsync(conn =>
            {
                return conn.ExecuteAsync($@"
                IF NOT EXISTS ( SELECT 1 FROM {TelegramSessionTable} WHERE {UserId} = @{UserId})
                BEGIN
                  INSERT INTO {TelegramSessionTable} ({UserId},{Session})
                  VALUES (@{UserId},@{Session})
                END
                	ELSE
                BEGIN
                  UPDATE {TelegramSessionTable}
                  SET {Session} = @{Session}
                  WHERE {UserId} = @{UserId}
                END;
            ", new { UserId = _userId, Session = session });
            });
        }

        public void SetSessionTag(string sessionTag)
        {
            _userId = sessionTag;
        }
    }
}
