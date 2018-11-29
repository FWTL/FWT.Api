using System;

namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class TelegramChat
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? CreateDate { get; set; }

        public int? MigratetToChannelId { get; set; }
    }
}