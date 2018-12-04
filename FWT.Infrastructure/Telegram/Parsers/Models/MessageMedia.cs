using OpenTl.Schema;
using System.Collections.Generic;
using static FWT.Core.Helpers.Enum;

namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class MessageMedia
    {
        public TelegramMediaType Type { get; set; }
        public List<File> Files { get; set; }
        public List<DocumentAttribute> Attibutes { get; set; } = new List<DocumentAttribute>();
    }
}