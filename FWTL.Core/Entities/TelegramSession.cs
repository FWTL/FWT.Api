namespace FWTL.Core.Entities
{
    public class TelegramSession
    {
        public string UserId { get; set; }

        public byte[] Session { get; set; }
    }
}
