using FWT.Core.Services.Telegram;
using FWT.Core.Services.Unique;
using FWT.Infrastructure.Telegram;
using FWT.Infrastructure.Telegram.Parsers;
using FWT.Infrastructure.Telegram.Parsers.Models;
using Hangfire;
using OpenTl.ClientApi;
using OpenTl.Schema;
using OpenTl.Schema.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FWT.Api.Jobs
{
    public class GetMessages
    {
        private readonly ITelegramService _telegramService;
        private readonly IRandomService _randomService;

        public GetMessages(ITelegramService telegramService, IRandomService randomService)
        {
            _telegramService = telegramService;
            _randomService = randomService;
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

            List<Message> messages = MessagesParser.Parse(history);

            if (messages.Count > 0)
            {
                BackgroundJob.Schedule<GetMessages>(
                    job => job.ForContactAsync(contactId, phoneHashId, offset + 100, 0),
                    TimeSpan.FromSeconds(_randomService.Random.Next(5, 20)));
            }
        }
    }
}