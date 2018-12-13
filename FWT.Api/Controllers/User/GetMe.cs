using System;
using System.Threading.Tasks;
using FluentValidation;
using FWT.Core.CQRS;
using FWT.Core.Services.Telegram;
using FWT.Infrastructure.Cache;
using FWT.Infrastructure.Handlers;
using FWT.Infrastructure.Telegram;
using FWT.Infrastructure.Validation;
using OpenTl.ClientApi;
using OpenTl.Schema;
using StackExchange.Redis;

namespace FWT.Api.Controllers.User
{
    public class GetMe
    {
        public class Cache : RedisJsonHandler<Query, Result>
        {
            public Cache(IDatabase cache) : base(cache)
            {
                KeyFn = query =>
                {
                    return CacheKeyBuilder.Build<GetMe, Query>(query, m => m.PhoneHashId);
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
                IClientApi client = await _telegramService.BuildAsync(query.PhoneHashId);

                TUserFull result = await TelegramRequest.HandleAsync(() =>
                {
                    return client.UsersService.GetCurrentUserFullAsync();
                });

                return new Result(result.User.As<TUser>());
            }
        }

        public class Query : IQuery
        {
            public string PhoneHashId { get; set; }
        }

        public class Result
        {
            public Result()
            {
            }

            public Result(TUser user)
            {
                FirstName = user.FirstName;
                LastName = user.LastName;
                UserName = user.Username;
                PhotoId = user.Photo.As<TUserProfilePhoto>()?.PhotoId;
            }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public long? PhotoId { get; set; }

            public string UserName { get; set; }
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
