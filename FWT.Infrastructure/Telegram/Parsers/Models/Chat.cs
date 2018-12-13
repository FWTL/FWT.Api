using System;

namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class Chat
    {
        public DateTime? CreateDate { get; set; }

        public int Id { get; set; }

        public int? MigratetToChannelId { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }
    }
}
