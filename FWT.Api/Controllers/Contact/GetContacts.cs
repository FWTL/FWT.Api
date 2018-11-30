using FluentValidation;
using FWT.Core.CQRS;
using FWT.Core.Services.Telegram;
using FWT.Infrastructure.Cache;
using FWT.Infrastructure.Handlers;
using FWT.Infrastructure.Telegram;
using FWT.Infrastructure.Telegram.Parsers.Models;
using FWT.Infrastructure.Validation;
using OpenTl.ClientApi;
using OpenTl.Schema;
using OpenTl.Schema.Contacts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FWT.Api.Controllers.Dialog
{
    public class GetContacts
    {
        public class Query : IQuery
        {
            public string PhoneHashId { get; set; }
        }

        public class Cache : RedisJsonHandler<Query, List<TelegramContact>>
        {
            public Cache(IDatabase cache) : base(cache)
            {
                KeyFn = query =>
                {
                    return CacheKeyBuilder.Build<TelegramContact, Query>(query, m => m.PhoneHashId);
                };
            }

            public override TimeSpan? Ttl(Query query)
            {
                return TimeSpan.FromHours(24);
            }
        }

        public class Handler : IQueryHandler<Query, List<TelegramContact>>
        {
            private readonly ITelegramService _telegramService;

            public Handler(ITelegramService telegramService)
            {
                _telegramService = telegramService;
            }

            public async Task<List<TelegramContact>> HandleAsync(Query query)
            {
                IClientApi client = await _telegramService.BuildAsync(query.PhoneHashId);
                TContacts result = (await TelegramRequest.Handle(() =>
                {
                    return client.ContactsService.GetContactsAsync();
                }));

                List<TelegramContact> contacts = result.Users.Select(c =>
                {
                    return new TelegramContact(c.As<TUser>());
                }).ToList();

                return contacts;
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