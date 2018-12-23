using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using FWTL.Core.CQRS;
using FWTL.Core.Helpers;
using FWTL.Core.Services.Telegram;
using FWTL.Infrastructure.Telegram;
using FWTL.Infrastructure.Validation;
using OpenTl.ClientApi;
using OpenTl.Schema.Auth;

namespace FWTL.Api.Controllers.Accounts
{
    public class SendCode
    {
        public class Handler : IQueryHandler<Query, string>
        {
            private readonly ITelegramService _telegramService;

            public Handler(ITelegramService telegramService)
            {
                _telegramService = telegramService;
            }

            public async Task<string> HandleAsync(Query query)
            {
                string hashedPhoneId = HashHelper.GetHash(query.PhoneNumber);
                IClientApi client = await _telegramService.BuildAsync(hashedPhoneId);
                ISentCode result = await TelegramRequest.HandleAsync(() =>
                {
                    return client.AuthService.SendCodeAsync(query.PhoneNumber);
                });

                return result.PhoneCodeHash;
            }
        }

        public class Query : IQuery
        {
            public Query(string phoneNumber)
            {
                PhoneNumber = Regex.Replace(phoneNumber ?? string.Empty, "[^0-9]", "");
            }

            public string PhoneNumber { get; }
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