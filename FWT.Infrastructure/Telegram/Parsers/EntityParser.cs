using System;
using System.Collections.Generic;
using FWT.Infrastructure.Telegram.Parsers.Models;
using OpenTl.Schema;
using static FWT.Core.Helpers.Enum;

namespace FWT.Infrastructure.Telegram.Parsers
{
    public static class EntityParser
    {
        private static readonly Dictionary<string, Func<IMessageEntity, MessageEntity>> Switch = new Dictionary<string, Func<IMessageEntity, MessageEntity>>()
        {
              { typeof(TMessageEntityMention).FullName, x => { return Parse(x as TMessageEntityMention); } },
              { typeof(TMessageEntityPhone).FullName, x => { return Parse(x as TMessageEntityPhone); } },
              { typeof(TMessageEntityBold).FullName, x => { return Parse(x as TMessageEntityBold); } },
              { typeof(TMessageEntityCashtag).FullName, x => { return Parse(x as TMessageEntityCashtag); } },
              { typeof(TMessageEntityTextUrl).FullName, x => { return Parse(x as TMessageEntityTextUrl); } },
              { typeof(TMessageEntityItalic).FullName, x => { return Parse(x as TMessageEntityItalic); } },
              { typeof(TMessageEntityPre).FullName, x => { return Parse(x as TMessageEntityPre); } },
              { typeof(TMessageEntityMentionName).FullName, x => { return Parse(x as TMessageEntityMentionName); } },
              { typeof(TMessageEntityEmail).FullName, x => { return Parse(x as TMessageEntityEmail); } },
              { typeof(TMessageEntityBotCommand).FullName, x => { return Parse(x as TMessageEntityBotCommand); } },
              { typeof(TMessageEntityUnknown).FullName, x => { return Parse(x as TMessageEntityUnknown); } },
              { typeof(TMessageEntityUrl).FullName, x => { return Parse(x as TMessageEntityUrl); } },
              { typeof(TMessageEntityHashtag).FullName, x => { return Parse(x as TMessageEntityHashtag); } },
              { typeof(TInputMessageEntityMentionName).FullName, x => { return Parse(x as TInputMessageEntityMentionName); } },
              { typeof(TMessageEntityCode).FullName, x => { return Parse(x as TMessageEntityCode); } },
        };

        public static MessageEntity Parse(IMessageEntity entity)
        {
            string key = entity.GetType().FullName;
            return Switch[key](entity);
        }

        private static MessageEntity Parse(TInputMessageEntityMentionName inputMessageEntityMentionName)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.MentionName
            };
        }

        private static MessageEntity Parse(TMessageEntityBold messageEntityBold)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.Bold
            };
        }

        private static MessageEntity Parse(TMessageEntityBotCommand messageEntityBotCommand)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.BotCommand
            };
        }

        private static MessageEntity Parse(TMessageEntityCashtag messageEntityCashtag)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.CashTag
            };
        }

        private static MessageEntity Parse(TMessageEntityCode messageEntityCode)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.Code
            };
        }

        private static MessageEntity Parse(TMessageEntityEmail messageEntityEmail)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.Email
            };
        }

        private static MessageEntity Parse(TMessageEntityHashtag messageEntityHashtag)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.Hashtag
            };
        }

        private static MessageEntity Parse(TMessageEntityItalic messageEntityItalic)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.Italic
            };
        }

        private static MessageEntity Parse(TMessageEntityMention messageEntityMention)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.Mention
            };
        }

        private static MessageEntity Parse(TMessageEntityMentionName messageEntityMentionName)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.MentionName
            };
        }

        private static MessageEntity Parse(TMessageEntityPhone messageEntityPhone)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.Phone
            };
        }

        private static MessageEntity Parse(TMessageEntityPre messageEntityPre)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.Pre
            };
        }

        private static MessageEntity Parse(TMessageEntityTextUrl messageEntityTextUrl)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.TextUrl
            };
        }

        private static MessageEntity Parse(TMessageEntityUnknown messageEntityUnknown)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.Unknown
            };
        }

        private static MessageEntity Parse(TMessageEntityUrl messageEntityUrl)
        {
            return new MessageEntity()
            {
                Type = TelegramEntity.Url
            };
        }
    }
}
