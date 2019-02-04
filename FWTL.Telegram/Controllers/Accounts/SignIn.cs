using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using FWTL.Core.CQRS;
using FWTL.Core.Helpers;
using FWTL.Core.Services.Telegram;
using FWTL.Infrastructure.Telegram;
using FWTL.Infrastructure.Validation;
using OpenTl.ClientApi;
using OpenTl.Schema;
using OpenTl.Schema.Auth;

namespace FWTL.Telegram.Controllers.Accounts
{
    public class SignIn
    {
        public class Handler : IQueryHandler<Query, TUser>
        {
            private readonly ITelegramService _telegramService;

            public Handler(ITelegramService telegramService)
            {
                _telegramService = telegramService;
            }

            public async Task<TUser> HandleAsync(Query query)
            {
                string hashedPhoneId = HashHelper.GetHash(query.PhoneNumber);
                IClientApi client = await _telegramService.BuildAsync(hashedPhoneId);

                var sentCode = new TSentCode()
                {
                    PhoneCodeHash = query.SentCode
                };

                TUser result = await TelegramRequest.HandleAsync(() =>
                {
                    return client.AuthService.SignInAsync(query.PhoneNumber, sentCode, query.Code);
                });

                return result;
            }
        }

        public class Query : IQuery
        {
            public Query(string phoneNumber, string sentCode, string code)
            {
                PhoneNumber = Regex.Replace(phoneNumber ?? string.Empty, "[^0-9]", "");
                Code = code;
                SentCode = sentCode;
            }

            public string Code { get; }

            public string PhoneNumber { get; }

            public string SentCode { get; }
        }

        public class Validator : AppAbstractValidation<Query>
        {
            public Validator()
            {
                RuleFor(x => x.PhoneNumber).NotEmpty();
                RuleFor(x => x.Code).NotEmpty();
                RuleFor(x => x.SentCode).NotEmpty();
            }
        }
    }
}