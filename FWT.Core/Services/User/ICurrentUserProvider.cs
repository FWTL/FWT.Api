using System.Security.Claims;

namespace FWT.Core.Services.User
{
    public interface ICurrentUserProvider
    {
        string PhoneHashId(ClaimsPrincipal user);
    }
}