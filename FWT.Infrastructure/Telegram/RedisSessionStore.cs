using OpenTl.ClientApi;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace FWT.Infrastructure.Telegram
{
    public class RedisSessionStore : ISessionStore
    {
        private readonly IDatabase _cache;
        private string _hashId;

        public RedisSessionStore(IDatabase cache)
        {
            _cache = cache;
        }

        public void Dispose()
        {
        }

        public byte[] Load()
        {
            RedisValue result = _cache.StringGet(_hashId);
            if (result.IsNullOrEmpty)
            {
                return null;
            }

            return result;
        }

        public async Task Save(byte[] session)
        {
            await _cache.StringSetAsync(_hashId, session, new TimeSpan(1, 0, 0));
        }

        public void SetSessionTag(string sessionTag)
        {
            _hashId = sessionTag;
        }
    }
}