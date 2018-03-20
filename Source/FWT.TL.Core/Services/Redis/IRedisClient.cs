using StackExchange.Redis;

namespace FWT.TL.Core.Services.Redis
{
    public interface IRedisClient
    {
        IDatabase Cache { get; }
    }
}