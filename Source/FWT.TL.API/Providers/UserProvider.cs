﻿using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using Autofac;
using FWT.TL.Core.Extensions;
using FWT.TL.Core.Providers;

namespace FWT.TL.API.Providers
{
    public class UserProvider : IUserProvider
    {
        private HttpRequestMessage _request;

        public UserProvider(HttpRequestMessage request)
        {
            _request = request;
        }

        public int CurrentUserId
        {
            get
            {
                ClaimsPrincipal principal = _request.GetRequestContext().Principal as ClaimsPrincipal;
                return principal.Claims.Where(c => c.Type == "as:UserId").Single().Value.To<int>();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return _request.GetRequestContext().Principal.Identity.IsAuthenticated;
            }
        }
    }
}