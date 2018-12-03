using FWT.Infrastructure.Telegram.Parsers.Models;
using OpenTl.Schema;
using System;
using System.Collections.Generic;

namespace FWT.Infrastructure.Telegram.Parsers
{
    public class EntityParser
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
        };

        private static MessageEntity Parse(TMessageEntityHashtag messageEntityHashtag)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityUrl messageEntityUrl)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityUnknown messageEntityUnknown)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityBotCommand messageEntityBotCommand)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityEmail messageEntityEmail)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityMentionName messageEntityMentionName)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityPre messageEntityPre)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityCode messageEntityCode)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityItalic messageEntityItalic)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityTextUrl messageEntityTextUrl)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityCashtag messageEntityCashtag)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityBold messageEntityBold)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityPhone messageEntityPhone)
        {
            throw new NotImplementedException();
        }

        private static MessageEntity Parse(TMessageEntityMention messageEntityMention)
        {
            throw new NotImplementedException();
        }

        public static MessageEntity Parse(IMessageEntity entity)
        {
            string key = entity.GetType().FullName;
            return Switch[key](entity);
        }
    }
}