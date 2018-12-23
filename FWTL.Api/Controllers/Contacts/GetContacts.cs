using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FWTL.Core.CQRS;
using FWTL.Core.Services.Telegram;
using FWTL.Infrastructure.Cache;
using FWTL.Infrastructure.Handlers;
using FWTL.Infrastructure.Telegram;
using FWTL.Infrastructure.Telegram.Parsers.Models;
using FWTL.Infrastructure.Validation;
using OpenTl.ClientApi;
using OpenTl.Schema;
using OpenTl.Schema.Contacts;
using StackExchange.Redis;

namespace FWTL.Api.Controllers.Contacts
{
    public class GetContacts
    {
        public class Cache : RedisJsonHandler<Query, List<Contact>>
        {
            public Cache(IDatabase cache) : base(cache)
            {
                KeyFn = query =>
                {
                    return CacheKeyBuilder.Build<Contact, Query>(query, m => m.PhoneHashId);
                };
            }

            public override TimeSpan? Ttl(Query query)
            {
                return TimeSpan.FromHours(24);
            }
        }

        public class Handler : IQueryHandler<Query, List<Contact>>
        {
            private readonly ITelegramService _telegramService;

            public Handler(ITelegramService telegramService)
            {
                _telegramService = telegramService;
            }

            public async Task<List<Contact>> HandleAsync(Query query)
            {
                IClientApi client = await _telegramService.BuildAsync(query.PhoneHashId);
                TContacts result = (await TelegramRequest.HandleAsync(() =>
                {
                    return client.ContactsService.GetContactsAsync();
                }));

                List<Contact> contacts = result.Users.Select(c =>
                {
                    return new Contact(c.As<TUser>());
                }).ToList();

                return contacts;
            }
        }

        public class Query : IQuery
        {
            public string PhoneHashId { get; set; }
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