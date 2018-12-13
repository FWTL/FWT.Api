using System;
using System.Collections.Generic;
using FWT.Infrastructure.Telegram.Parsers.Models;
using OpenTl.Schema.Messages;
using static FWT.Core.Helpers.Enum;

namespace FWT.Infrastructure.Telegram.Parsers
{
    public static class MessagesParser
    {
        private static readonly Dictionary<string, Func<IMessages, List<Message>>> Switch = new Dictionary<string, Func<IMessages, List<Message>>>()
        {
              { typeof(TMessagesSlice).FullName, x => { return Parse(x as TMessagesSlice); } },
              { typeof(TMessagesNotModified).FullName, x => { return Parse(x as TMessagesNotModified); } },
              { typeof(TChannelMessages).FullName, x => { return Parse(x as TChannelMessages); } },
              { typeof(TMessages).FullName, x => { return Parse(x as TMessages); } },
        };

        public static List<Message> Parse(IMessages messages, int id, PeerType peerType)
        {
            string key = messages.GetType().FullName;
            List<Message> message = Switch[key](messages);
            message.ForEach(m =>
            {
                m.UniqueId = $"{m.Id};{id};{(int)peerType}";
                m.SourceId = $"{id};{(int)peerType}";
            });

            return message;
        }

        private static List<Message> Parse(TChannelMessages channelMessages)
        {
            var result = new List<Message>();
            foreach (var message in channelMessages.Messages)
            {
                result.Add(MessageParser.Parse(message));
            }

            return result;
        }

        private static List<Message> Parse(TMessages messages)
        {
            var result = new List<Message>();
            foreach (var message in messages.Messages)
            {
                result.Add(MessageParser.Parse(message));
            }

            return result;
        }

        private static List<Message> Parse(TMessagesNotModified messagesNotModified)
        {
            throw new NotImplementedException();
        }

        private static List<Message> Parse(TMessagesSlice messagesSlice)
        {
            var result = new List<Message>();
            foreach (var message in messagesSlice.Messages)
            {
                result.Add(MessageParser.Parse(message));
            }

            return result;
        }
    }
}
