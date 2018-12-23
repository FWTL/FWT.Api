using System;
using System.Collections.Generic;
using FWTL.Infrastructure.Telegram.Parsers.Models;
using NodaTime;
using OpenTl.Schema;

namespace FWTL.Infrastructure.Telegram.Parsers
{
    public static class ChatParser
    {
        private static readonly Dictionary<string, Func<IChat, Channel>> SwitchChannel = new Dictionary<string, Func<IChat, Channel>>()
        {
              { typeof(TChannel).FullName, x => { return Parse(x as TChannel); } },
              { typeof(TChannelForbidden).FullName, x => { return Parse(x as TChannelForbidden); } },
        };

        private static readonly Dictionary<string, Func<IChat, Chat>> SwitchChat = new Dictionary<string, Func<IChat, Chat>>()
        {
              { typeof(TChatEmpty).FullName, x => { return Parse(x as TChatEmpty); } },
              { typeof(TChat).FullName, x => { return Parse(x as TChat); } },
              { typeof(TChatForbidden).FullName, x => { return Parse(x as TChatForbidden); } },
        };

        public static Channel ParseChannel(IChat chat)
        {
            string key = chat.GetType().FullName;
            if (SwitchChannel.ContainsKey(key))
            {
                return SwitchChannel[key](chat);
            }

            return null;
        }

        public static Chat ParseChat(IChat chat)
        {
            string key = chat.GetType().FullName;
            if (SwitchChat.ContainsKey(key))
            {
                return SwitchChat[key](chat);
            }

            return null;
        }

        private static Channel Parse(TChannel chat)
        {
            var appChat = new Channel()
            {
                Id = chat.Id,
                Title = chat.Title,
                CreateDate = Instant.FromUnixTimeSeconds(chat.Date).ToDateTimeUtc()
            };

            return appChat;
        }

        private static Channel Parse(TChannelForbidden chat)
        {
            var appChat = new Channel()
            {
                Id = chat.Id,
                Title = chat.Title,
            };

            return appChat;
        }

        private static Chat Parse(TChat chat)
        {
            var appChat = new Chat()
            {
                Id = chat.Id,
                CreateDate = Instant.FromUnixTimeSeconds(chat.Date).ToDateTimeUtc(),
                Title = chat.Title,
                MigratetToChannelId = chat.MigratedTo.As<TInputChannel>()?.ChannelId
            };

            return appChat;
        }

        private static Chat Parse(TChatEmpty chat)
        {
            throw new NotImplementedException();
        }

        private static Chat Parse(TChatForbidden chat)
        {
            var appChat = new Chat()
            {
                Id = chat.Id,
                Title = chat.Title,
            };

            return appChat;
        }
    }
}
