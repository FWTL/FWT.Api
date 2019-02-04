using System.Threading.Tasks;
using FluentValidation;
using FWTL.Core.CQRS;
using FWTL.Core.Services.Telegram;
using FWTL.Infrastructure.Telegram;
using FWTL.Infrastructure.Validation;
using OpenTl.ClientApi;
using OpenTl.Schema;

namespace FWTL.Telegram.Controllers.Accounts
{
    public class Logout
    {
        public class Handler : IQueryHandler<Query, bool>
        {
            private readonly ITelegramService _telegramService;

            public Handler(ITelegramService telegramService)
            {
                _telegramService = telegramService;
            }

            public async Task<bool> HandleAsync(Query query)
            {
                IClientApi client = await _telegramService.BuildAsync(query.UserId);

                await TelegramRequest.HandleAsync(() =>
                {
                    return client.AuthService.LogoutAsync();
                });

                return true;
            }
        }

        public class Query : IQuery
        {
            public string UserId { get; set; }
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

            public long? PhotoId { get; }

            public string UserName { get; }
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
