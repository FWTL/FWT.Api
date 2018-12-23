using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FWTL.Core.CQRS;
using FWTL.Core.Extensions;
using FWTL.Core.Services.Telegram;
using FWTL.Infrastructure.Cache;
using FWTL.Infrastructure.Handlers;
using FWTL.Infrastructure.Telegram;
using FWTL.Infrastructure.Telegram.Parsers;
using FWTL.Infrastructure.Telegram.Parsers.Models;
using FWTL.Infrastructure.Validation;
using OpenTl.ClientApi;
using OpenTl.Schema;
using OpenTl.Schema.Messages;
using StackExchange.Redis;

namespace FWTL.Api.Controllers.Channels
{
    public class GetChannels
    {
        public class Cache : RedisJsonHandler<Query, List<Channel>>
        {
            public Cache(IDatabase cache) : base(cache)
            {
                KeyFn = query =>
                {
                    return CacheKeyBuilder.Build<GetChannels, Query>(query, m => m.UserId);
                };
            }

            public override TimeSpan? Ttl(Query query)
            {
                return TimeSpan.FromHours(24);
            }
        }

        public class Handler : IQueryHandler<Query, List<Channel>>
        {
            private readonly ITelegramService _telegramService;

            public Handler(ITelegramService telegramService)
            {
                _telegramService = telegramService;
            }

            public async Task<List<Channel>> HandleAsync(Query query)
            {
                IClientApi client = await _telegramService.BuildAsync(query.UserId);

                TDialogs result = (await TelegramRequest.HandleAsync(() =>
                {
                    return client.MessagesService.GetUserDialogsAsync();
                })).As<TDialogs>();

                var channels = new List<Channel>();
                foreach (IChat channel in result.Chats)
                {
                    channels.AddWhenNotNull(ChatParser.ParseChannel(channel));
                }

                return channels;
            }
        }

        public class Query : IQuery
        {
            public string UserId { get; set; }
        }

        public class Validator : AppAbstractValidation<Query>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty();
            }
        }
    }
}