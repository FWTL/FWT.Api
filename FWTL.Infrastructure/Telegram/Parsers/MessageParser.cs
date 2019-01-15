using FWTL.Core.Extensions;
using FWTL.Events.Telegram.Messages;
using NodaTime;
using OpenTl.Schema;
using System;
using System.Collections.Generic;

namespace FWTL.Infrastructure.Telegram.Parsers
{
    public static class MessageParser
    {
        private static readonly Dictionary<string, Func<IMessage, Message>> Switch = new Dictionary<string, Func<IMessage, Message>>()
        {
              { typeof(TMessage).FullName, x => { return Parse(x as TMessage); } },
              { typeof(TMessageService).FullName, x => { return Parse(x as TMessageService); } },
              { typeof(TMessageEmpty).FullName, x => { return Parse(x as TMessageEmpty); } },
        };

        public static Message Parse(IMessage message)
        {
            string key = message.GetType().FullName;
            return Switch[key](message);
        }

        private static Message Parse(TMessage message)
        {
            var parsedMessage = new Message()
            {
                Id = message.Id,
                CreateDate = Instant.FromUnixTimeSeconds(message.Date).ToDateTimeUtc(),
                EditDate = message.EditDate > 0 ? Instant.FromUnixTimeSeconds(message.EditDate).ToDateTimeUtc() : (DateTime?)null,
                FromId = message.FromId,
                Text = message.Message,
                Media = MediaParser.Parse(message.Media),
            };

            message.Entities?.ForEach(entity => parsedMessage.Entities.Add(EntityParser.Parse(entity)));
            return parsedMessage;
        }

        private static Message Parse(TMessageEmpty messageEmpty)
        {
            throw new NotImplementedException();
        }

        private static Message Parse(TMessageService messageService)
        {
            var message = MessageServiceParser.Parse(messageService.Action);
            message.Id = messageService.Id;
            message.CreateDate = Instant.FromUnixTimeSeconds(messageService.Date).ToDateTimeUtc();
            return message;
        }
    }
}