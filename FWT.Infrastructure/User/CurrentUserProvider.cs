using System.Linq;
using System.Security.Claims;
using FWT.Core.Services.User;

namespace FWT.Infrastructure.User
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        public string PhoneHashId(ClaimsPrincipal user)
        {
            return user.Claims.First(c => c.Type == "PhoneHashId").Value;
        }
    }
}
