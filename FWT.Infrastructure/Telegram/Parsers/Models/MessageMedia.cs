using System.Collections.Generic;
using static FWT.Core.Helpers.Enum;

namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class MessageMedia
    {
        public List<DocumentAttribute> Attibutes { get; set; } = new List<DocumentAttribute>();

        public List<File> Files { get; set; }

        public TelegramMediaType Type { get; set; }
    }
}
