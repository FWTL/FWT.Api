using OpenTl.Schema;

namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class PhotoSize
    {
        public IFileLocation Location { get; set; }

        public int Size { get; set; }
    }
}
