using OpenTl.Schema;

namespace FWTL.Infrastructure.Telegram.Parsers.Models
{
    public class Contact
    {
        public Contact()
        {
        }

        public Contact(TUser contact)
        {
            Id = contact.Id;
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            UserName = contact.Username;
        }

        public string FirstName { get; set; }

        public int Id { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }
    }
}
