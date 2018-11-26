﻿using FluentValidation;
using FluentValidation.Results;
using FWT.Core.Helpers;
using FWT.Core.Services.Identity;
using IdentityModel.Client;
using OpenTl.Schema;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FWT.Infrastructure.Identity
{
    public class IdentityModelClient : IIdentityModelClient
    {
        private IdentityModelCredentials _credentials;
        private IDiscoveryCache _cache;

        public IdentityModelClient(IdentityModelCredentials credentials, IDiscoveryCache cache)
        {
            _credentials = credentials;
            _cache = cache;
        }

        public async Task<TokenResponse> RequestClientCredentialsTokenAsync(TUser user)
        {
            var disco = await _cache.GetAsync();

            var client = new HttpClient();
            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = _credentials.ClientId,
                ClientSecret = _credentials.ClientSecret,
                Scope = "api",
                Parameters = new Dictionary<string, string>()
                {
                    { "PhoneHashId", HashHelper.GetHash(user.Phone) }
                }
            });

            if (response.IsError)
            {
                throw new ValidationException(new List<ValidationFailure>()
                {
                    new ValidationFailure("request", response.Error)
                });
            }

            return response;
        }
    }
}