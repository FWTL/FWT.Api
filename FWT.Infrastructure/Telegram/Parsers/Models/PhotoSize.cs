using OpenTl.Schema;

namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class PhotoSize
    {
        public int Size { get; set; }
        public IFileLocation Location { get; set; }
    }
}