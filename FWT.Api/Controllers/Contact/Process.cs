using FluentValidation;
using FWT.Core.CQRS;
using FWT.Core.Services.Telegram;
using FWT.Infrastructure.Telegram;
using FWT.Infrastructure.Validation;
using OpenTl.ClientApi;
using OpenTl.Schema;
using OpenTl.Schema.Messages;
using System.Threading.Tasks;

namespace FWT.Api.Controllers.Dialog
{
    public class Process
    {
        public class Command : ICommand
        {
            public int ContactId { get; set; }
            public string PhoneHashId { get; set; }
        }

        public class Handler : ICommandHandler<Command>
        {
            private readonly ITelegramService _telegramService;

            public Handler(ITelegramService telegramService)
            {
                _telegramService = telegramService;
            }

            public async Task ExecuteAsync(Command command)
            {
                IClientApi client = await _telegramService.BuildAsync(command.PhoneHashId);
                IMessages history = await TelegramRequest.Handle(() =>
                {
                    return client.MessagesService.GetHistoryAsync(new TInputPeerUser()
                    {
                        UserId = command.ContactId,
                    }, 0, 0, 100);
                });
            }
        }

        public class Validator : AppAbstractValidation<Command>
        {
            public Validator()
            {
                RuleFor(x => x.PhoneHashId).NotEmpty();
            }
        }
    }
}