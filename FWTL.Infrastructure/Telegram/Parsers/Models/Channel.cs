using System;

namespace FWTL.Infrastructure.Telegram.Parsers.Models
{
    public class Channel
    {
        public DateTime? CreateDate { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }
    }
}
