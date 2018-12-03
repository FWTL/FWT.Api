using FluentValidation;
using FWT.Core.CQRS;
using FWT.Core.Extensions;
using FWT.Core.Services.Telegram;
using FWT.Infrastructure.Cache;
using FWT.Infrastructure.Handlers;
using FWT.Infrastructure.Telegram;
using FWT.Infrastructure.Telegram.Parsers;
using FWT.Infrastructure.Telegram.Parsers.Models;
using FWT.Infrastructure.Validation;
using OpenTl.ClientApi;
using OpenTl.Schema;
using OpenTl.Schema.Messages;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FWT.Api.Controllers.Chat
{
    public class GetChats
    {
        public class Query : IQuery
        {
            public string PhoneHashId { get; set; }
        }

        public class Cache : RedisJsonHandler<Query, List<Infrastructure.Telegram.Parsers.Models.Chat>>
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

        public class Handler : IQueryHandler<Query, List<Infrastructure.Telegram.Parsers.Models.Chat>>
        {
            private readonly ITelegramService _telegramService;

            public Handler(ITelegramService telegramService)
            {
                _telegramService = telegramService;
            }

            public async Task<List<Infrastructure.Telegram.Parsers.Models.Chat>> HandleAsync(Query query)
            {
                IClientApi client = await _telegramService.BuildAsync(query.PhoneHashId);

                TDialogs result = (await TelegramRequest.Handle(() =>
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

        public class Validator : AppAbstractValidation<Query>
        {
            public Validator()
            {
                RuleFor(x => x.PhoneHashId).NotEmpty();
            }
        }
    }
}