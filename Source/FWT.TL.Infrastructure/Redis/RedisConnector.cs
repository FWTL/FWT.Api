using FWT.TL.Core;
using StackExchange.Redis;
using System;

namespace FWT.TL.Infrastructure.Redis
{
    public static class RedisConnector
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(ConfigKeys.RedisConnectionString);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}