using OpenTl.Schema;

namespace FWT.Infrastructure.Telegram.Parsers.Models
{
    public class Contact
    {
        
        public Contact(TUser contact)
        {
            Id = contact.Id;
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            UserName = contact.Username;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
    }
}