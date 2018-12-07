using FluentValidation;
using FWT.Api.Jobs;
using FWT.Core.CQRS;
using FWT.Infrastructure.Validation;
using Hangfire;
using System.Threading.Tasks;
using static FWT.Core.Helpers.Enum;

namespace FWT.Api.Controllers.Message
{
    public class Process
    {
        public class Command : ICommand
        {
            public int Id { get; set; }
            public string PhoneHashId { get; set; }
            public PeerType Type { get; set; }
        }

        public class Handler : ICommandHandler<Command>
        {
            public Task ExecuteAsync(Command command)
            {
                BackgroundJob.Enqueue<GetMessages>(job => job.ForPeer(command.Id, command.Type, command.PhoneHashId, 0, 0));
                return Task.CompletedTask;
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