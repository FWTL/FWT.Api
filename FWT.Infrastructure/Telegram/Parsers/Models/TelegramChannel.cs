using System;

namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class TelegramChannel
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}