﻿using FluentValidation;
using FWT.Core.CQRS;
using FWT.Core.Extensions;
using FWT.Core.Helpers;
using FWT.Core.Services.Telegram;
using FWT.Infrastructure.Telegram;
using FWT.Infrastructure.Validation;
using NodaTime;
using OpenTl.ClientApi;
using OpenTl.Schema.Auth;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FWT.Api.Controllers.Account
{
    public class SendCode
    {
        public class Query : IQuery
        {
            public Query(string phoneNumber)
            {
                PhoneNumber = phoneNumber.IsNotNull() ? $"+{Regex.Match(phoneNumber, @"\d+").Value}" : string.Empty;
            }

            public string PhoneNumber { get; }
        }

        public class Handler : IQueryHandler<Query, string>
        {
            private readonly IClock _clock;
            private readonly ITelegramService _telegramService;

            public Handler(IClock clock, ITelegramService telegramService)
            {
                _clock = clock;
                _telegramService = telegramService;
            }

            public async Task<string> HandleAsync(Query query)
            {
                string hashedPhoneId = HashHelper.GetHash(query.PhoneNumber);
                IClientApi client = await _telegramService.Build(hashedPhoneId);
                ISentCode result = await TelegramRequest.Handle(() =>
                {
                    return client.AuthService.SendCodeAsync(query.PhoneNumber);
                });

                return result.PhoneCodeHash;
            }
        }

        public class Validator : AppAbstractValidation<Query>
        {
            public Validator()
            {
                RuleFor(x => x.PhoneNumber).NotEmpty();
            }
        }
    }
}