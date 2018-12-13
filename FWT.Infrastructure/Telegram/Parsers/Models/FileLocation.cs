using OpenTl.Schema;

namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class File
    {
        public IInputFileLocation Location { get; set; }

        public int Size { get; set; }
    }
}
