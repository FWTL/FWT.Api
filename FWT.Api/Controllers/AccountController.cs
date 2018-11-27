using FWT.Api.Controllers.Account;
using FWT.Core.CQRS;
using FWT.Core.Services.Identity;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OpenTl.Schema;
using System.Threading.Tasks;

namespace FWT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IIdentityModelClient _identityClient;

        public AccountController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IIdentityModelClient identityClient)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _identityClient = identityClient;
        }

        [HttpPost]
        [Route("Sendcode")]
        public async Task<string> SendCode(string phoneNumber)
        {
            return await _queryDispatcher.DispatchAsync<SendCode.Query, string>(new SendCode.Query(phoneNumber));
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<JObject> SignIn(string phoneNumber, string sentCode, string code)
        {
            TUser tlUser = await _queryDispatcher.DispatchAsync<SignIn.Query, TUser>(new SignIn.Query(phoneNumber, sentCode, code));

            TokenResponse response = await _identityClient.RequestClientCredentialsTokenAsync(tlUser);
            return response.Json;
        }
    }
}