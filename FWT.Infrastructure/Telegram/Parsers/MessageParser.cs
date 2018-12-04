using FWT.Core.Extensions;
using FWT.Infrastructure.Telegram.Parsers.Models;
using NodaTime;
using OpenTl.Schema;
using System;
using System.Collections.Generic;

namespace FWT.Infrastructure.Telegram.Parsers
{
    public class MessageParser
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

        private static Message Parse(TMessageEmpty messageEmpty)
        {
            throw new NotImplementedException();
        }

        private static Message Parse(TMessageService messageService)
        {
            return new Message();
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
    }
}