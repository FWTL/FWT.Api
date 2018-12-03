namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class DocumentAttribute
    {
        
        public DocumentAttribute(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}