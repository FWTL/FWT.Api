using FWTL.Core.Services.EventHub;
using FWTL.Core.Services.Telegram;
using FWTL.Core.Services.Unique;
using FWTL.Events.Telegram.Messages;
using FWTL.Infrastructure.Telegram;
using FWTL.Infrastructure.Telegram.Parsers;
using Hangfire;
using OpenTl.ClientApi;
using OpenTl.Schema;
using OpenTl.Schema.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FWTL.Events.Telegram.Enums;

namespace FWTL.Api.Jobs
{
    public class GetMessages
    {
        private readonly IEventHubService _eventHub;

        private readonly IRandomService _randomService;

        private readonly ITelegramService _telegramService;

        public GetMessages(ITelegramService telegramService, IRandomService randomService, IEventHubService eventHub)
        {
            _telegramService = telegramService;
            _randomService = randomService;
            _eventHub = eventHub;
        }

        public async Task ForPeer(int id, PeerType peerType, string userId, int offset, int maxId)
        {
            IInputPeer peer = GetPeer(id, peerType);
            IClientApi client = await _telegramService.BuildAsync(userId);
            IMessages history = await TelegramRequest.HandleAsync(() =>
            {
                return client.MessagesService.GetHistoryAsync(peer, offset, maxId, 100);
            });

            List<Message> messages = MessagesParser.Parse(history, id, peerType);
            await _eventHub.SendAsync(messages);

            if (messages.Count > 0)
            {
                BackgroundJob.Schedule<GetMessages>(
                    job => job.ForPeer(id, peerType, userId, offset + 100, 0),
                    TimeSpan.FromSeconds(_randomService.Random.Next(5, 20)));
            }
        }

        private IInputPeer GetPeer(int id, PeerType peerType)
        {
            switch (peerType)
            {
                case (PeerType.Channal):
                    {
                        return new TInputPeerChannel()
                        {
                            ChannelId = id,
                        };
                    }
                case (PeerType.Chat):
                    {
                        return new TInputPeerChat()
                        {
                            ChatId = id
                        };
                    }
                case (PeerType.User):
                    {
                        return new TInputPeerUser()
                        {
                            UserId = id
                        };
                    }
                default:
                    {
                        throw new NotImplementedException($"{peerType} not implemented");
                    }
            }
        }
    }
}