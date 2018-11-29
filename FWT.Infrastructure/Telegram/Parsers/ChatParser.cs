using FWT.Infrastructure.Telegram.Parsers.Models;
using NodaTime;
using OpenTl.Schema;
using System;
using System.Collections.Generic;

namespace FWT.Infrastructure.Telegram.Parsers
{
    public class ChatParser
    {
        private static readonly Dictionary<string, Func<IChat, TelegramChat>> SwitchChat = new Dictionary<string, Func<IChat, TelegramChat>>()
        {
              { typeof(TChatEmpty).FullName, x => { return Parse(x as TChatEmpty); } },
              { typeof(TChat).FullName, x => { return Parse(x as TChat); } },
              { typeof(TChatForbidden).FullName, x => { return Parse(x as TChatForbidden); } },
        };

        private static readonly Dictionary<string, Func<IChat, TelegramChannel>> SwitchChannel = new Dictionary<string, Func<IChat, TelegramChannel>>()
        {
              { typeof(TChannel).FullName, x => { return Parse(x as TChannel); } },
              { typeof(TChannelForbidden).FullName, x => { return Parse(x as TChannelForbidden); } },
        };

        public static TelegramChat ParseChat(IChat chat)
        {
            string key = chat.GetType().FullName;
            if (SwitchChat.ContainsKey(key))
            {
                return SwitchChat[key](chat);
            }

            return null;
        }

        public static TelegramChannel ParseChannel(IChat chat)
        {
            string key = chat.GetType().FullName;
            if (SwitchChannel.ContainsKey(key))
            {
                return SwitchChannel[key](chat);
            }

            return null;
        }

        private static TelegramChat Parse(TChatEmpty chat)
        {
            var appChat = new TelegramChat()
            {
                Id = chat.Id,
                Title = "Empty Chat"
            };

            return appChat;
        }

        private static TelegramChat Parse(TChat chat)
        {
            var appChat = new TelegramChat()
            {
                Id = chat.Id,
                CreateDate = Instant.FromUnixTimeSeconds(chat.Date).ToDateTimeUtc(),
                Title = chat.Title,
                MigratetToChannelId = chat.MigratedTo.As<TInputChannel>()?.ChannelId
            };

            return appChat;
        }

        private static TelegramChat Parse(TChatForbidden chat)
        {
            var appChat = new TelegramChat()
            {
                Id = chat.Id,
                Title = chat.Title,
            };

            return appChat;
        }

        private static TelegramChannel Parse(TChannel chat)
        {
            var appChat = new TelegramChannel()
            {
                Id = chat.Id,
                Title = chat.Title,
                CreateDate = Instant.FromUnixTimeSeconds(chat.Date).ToDateTimeUtc()
            };

            return appChat;
        }

        private static TelegramChannel Parse(TChannelForbidden chat)
        {
            var appChat = new TelegramChannel()
            {
                Id = chat.Id,
                Title = chat.Title,
            };

            return appChat;
        }
    }
}