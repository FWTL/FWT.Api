namespace FWT.Api.Controllers.Chat
{
    using FluentValidation;
    using global::FWT.Core.CQRS;
    using global::FWT.Core.Services.Telegram;
    using global::FWT.Infrastructure.Cache;
    using global::FWT.Infrastructure.Handlers;
    using OpenTl.ClientApi;
    using OpenTl.Schema;
    using StackExchange.Redis;
    using System;
    using System.Threading.Tasks;

    namespace FWT.Api.Controllers.User
    {
        public class GetChats
        {
            public class Query : IQuery
            {
                public string PhoneHashId { get; set; }
            }

            public class Cache : RedisJsonHandler<Query, Result>
            {
                public Cache(IDatabase cache) : base(cache)
                {
                    KeyFn = query =>
                    {
                        return CacheKeyBuilder.Build<GetChats, Query>(query, m => m.PhoneHashId);
                    };
                }

                public override TimeSpan? Ttl(Query query)
                {
                    return TimeSpan.FromHours(24);
                }
            }

            public class Handler : IQueryHandler<Query, Result>
            {
                private readonly ITelegramService _telegramService;

                public Handler(ITelegramService telegramService)
                {
                    _telegramService = telegramService;
                }

                public async Task<Result> HandleAsync(Query query)
                {
                    IClientApi client = await _telegramService.Build(query.PhoneHashId);
                    TUserFull result = await client.UsersService.GetCurrentUserFullAsync();
                    return new Result(result.User.As<TUser>());
                }
            }

            public class Result
            {
                public Result(TUser user)
                {
                    FirstName = user.FirstName;
                    LastName = user.LastName;
                    UserName = user.Username;
                    PhotoId = user.Photo.As<TUserProfilePhoto>()?.PhotoId;
                }

                public string FirstName { get; }
                public string LastName { get; }
                public string UserName { get; }
                public long? PhotoId { get; }
            }

            public class Validator : AppAbstractValidation<Query>
            {
                public Validator()
                {
                    RuleFor(x => x.PhoneHashId).NotEmpty();
                }
            }
        }
    }
}