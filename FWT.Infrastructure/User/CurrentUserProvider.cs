using FWT.Core.Services.User;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace FWT.Infrastructure.User
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        public CurrentUserProvider()
        {
        }

        public string PhoneHashId(ClaimsPrincipal user)
        {
            return user.Claims.First(c => c.Type == "PhoneHashId").Value;
        }
    }
}