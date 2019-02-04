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
using FWTL.Infrastructure.Validation;
using OpenTl.ClientApi;
using OpenTl.Schema;
using OpenTl.Schema.Messages;
using StackExchange.Redis;

namespace FWTL.Telegram.Controllers.Chats
{
    public class GetChats
    {
        public class Cache : RedisJsonHandler<Query, List<Infrastructure.Telegram.Parsers.Models.Chat>>
        {
            public Cache(IDatabase cache) : base(cache)
            {
                KeyFn = query =>
                {
                    return CacheKeyBuilder.Build<GetChats, Query>(query, m => m.UserId);
                };
            }

            public override TimeSpan? Ttl(Query query)
            {
                return TimeSpan.FromHours(24);
            }
        }

        public class Handler : IQueryHandler<Query, List<Infrastructure.Telegram.Parsers.Models.Chat>>
        {
            private readonly ITelegramService _telegramService;

            public Handler(ITelegramService telegramService)
            {
                _telegramService = telegramService;
            }

            public async Task<List<Infrastructure.Telegram.Parsers.Models.Chat>> HandleAsync(Query query)
            {
                IClientApi client = await _telegramService.BuildAsync(query.UserId);

                TDialogs result = (await TelegramRequest.HandleAsync(() =>
                {
                    return client.MessagesService.GetUserDialogsAsync();
                })).As<TDialogs>();

                var chats = new List<Infrastructure.Telegram.Parsers.Models.Chat>();
                foreach (IChat chat in result.Chats)
                {
                    chats.AddWhenNotNull(ChatParser.ParseChat(chat));
                }

                return chats;
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
