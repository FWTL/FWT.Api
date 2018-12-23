using System;
using System.Collections.Generic;
using static FWTL.Core.Helpers.Enum;

namespace FWTL.Infrastructure.Telegram.Parsers.Models
{
    public class Message
    {
        public TelegramMessageAction Action { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? EditDate { get; set; }

        public List<MessageEntity> Entities { get; set; } = new List<MessageEntity>();

        public int FromId { get; set; }

        public int Id { get; set; }

        public MessageMedia Media { get; set; }

        public string SourceId { get; set; }

        public string Text { get; set; }

        public string UniqueId { get; set; }
    }
}
