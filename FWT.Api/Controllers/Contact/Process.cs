using FluentValidation;
using FWT.Api.Jobs;
using FWT.Core.CQRS;
using FWT.Infrastructure.Validation;
using Hangfire;
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
            public Task ExecuteAsync(Command command)
            {
                BackgroundJob.Enqueue<GetMessages>(job => job.ForContactAsync(command.ContactId, command.PhoneHashId, 0, 0));
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