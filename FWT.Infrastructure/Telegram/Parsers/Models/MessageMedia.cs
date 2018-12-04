using static FWT.Core.Helpers.Enum;

namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class MessageMedia
    {
        public TelegramMediaType Type { get; set; }
        public long Id { get; internal set; }
    }
}