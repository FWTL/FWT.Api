using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using FWTL.Telegram.Jobs;
using FWTL.Core.CQRS;
using FWTL.Core.Services.Telegram;
using FWTL.Infrastructure.Telegram;
using FWTL.Infrastructure.Validation;
using Hangfire;
using OpenTl.ClientApi;
using OpenTl.Schema;
using OpenTl.Schema.Channels;
using OpenTl.Schema.Messages;
using OpenTl.Schema.Users;
using static FWTL.Core.Helpers.Enum;

namespace FWTL.Telegram.Controllers.Messages
{
    public class Process
    {
        public class Command : ICommand
        {
            public int Id { get; set; }

            public string UserId { get; set; }

            public PeerType Type { get; set; }
        }

        public class Handler : ICommandHandler<Command>
        {
            public Task ExecuteAsync(Command command)
            {
                BackgroundJob.Enqueue<GetMessages>(job => job.ForPeer(command.Id, command.Type, command.UserId, 0, 0));
                return Task.CompletedTask;
            }
        }

        public class Validator : AppAbstractValidation<Command>
        {
            private readonly ITelegramService _telegramService;

            public Validator(ITelegramService telegramService)
            {
                _telegramService = telegramService;

                RuleFor(x => x.UserId).NotEmpty();
                RuleFor(x => x).CustomAsync(async (command, context, token) =>
                {
                    await HasAccessToPeerAsync(command, context).ConfigureAwait(false);
                });
            }

            private async Task HasAccessToPeerAsync(Command command, CustomContext context)
            {
                switch (command.Type)
                {
                    case (PeerType.Channal):
                        {
                            await ValidateRequestAsync(new RequestGetFullChannel() { Channel = new TInputChannel() { ChannelId = command.Id } }, command, context).ConfigureAwait(false);
                            return;
                        }
                    case (PeerType.Chat):
                        {
                            await ValidateRequestAsync(new RequestGetFullChat() { ChatId = command.Id }, command, context).ConfigureAwait(false);
                            return;
                        }
                    case (PeerType.User):
                        {
                            await ValidateRequestAsync(new RequestGetFullUser() { Id = new TInputUser() { UserId = command.Id, } }, command, context).ConfigureAwait(false);
                            return;
                        }
                    default:
                        {
                            throw new NotImplementedException($"{command.Type} not implemented");
                        }
                }
            }

            private async Task ValidateRequestAsync<TResult>(IRequest<TResult> request, Command command, CustomContext context)
            {
                IClientApi client = await _telegramService.BuildAsync(command.UserId);
                var result = (await TelegramRequest.HandleAsync(() =>
                {
                    return client.CustomRequestsService.SendRequestAsync(request);
                }, context));
            }
        }
    }
}
