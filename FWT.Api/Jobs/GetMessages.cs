using FWT.Core.Services.Telegram;
using FWT.Infrastructure.Telegram;
using Hangfire;
using OpenTl.ClientApi;
using OpenTl.Schema;
using OpenTl.Schema.Messages;
using System.Threading.Tasks;

namespace FWT.Api.Jobs
{
    public class GetMessages
    {
        private readonly ITelegramService _telegramService;

        public GetMessages(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        public async Task ForContactAsync(int contactId, string phoneHashId, int offset, int maxId)
        {
            IClientApi client = await _telegramService.BuildAsync(phoneHashId);
            IMessages history = await TelegramRequest.Handle(() =>
            {
                return client.MessagesService.GetHistoryAsync(new TInputPeerUser()
                {
                    UserId = contactId
                }, offset, maxId, 100);
            });

            BackgroundJob.Enqueue<GetMessages>(job => job.ForContactAsync(contactId, phoneHashId, offset + 100, 0));
        }
    }
}