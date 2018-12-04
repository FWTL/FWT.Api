using System;
using System.Collections.Generic;
using static FWT.Core.Helpers.Enum;

namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
        public int FromId { get; set; }
        public string Text { get; set; }
        public MessageMedia Media { get; set; }
        public List<MessageEntity> Entities { get; set; } = new List<MessageEntity>();

        public TelegramMessageAction Action { get; set; }
    }
}