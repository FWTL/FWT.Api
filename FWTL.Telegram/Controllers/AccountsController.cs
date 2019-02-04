using System.Threading.Tasks;
using FWTL.Telegram.Controllers.Accounts;
using FWTL.Core.CQRS;
using FWTL.Core.Services.Identity;
using FWTL.Core.Services.User;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OpenTl.Schema;

namespace FWTL.Telegram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        private readonly IIdentityModelClient _identityClient;

        private readonly IQueryDispatcher _queryDispatcher;

        private readonly ICurrentUserProvider _userProvider;

        public AccountsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, IIdentityModelClient identityClient, ICurrentUserProvider userProvider)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _identityClient = identityClient;
            _userProvider = userProvider;
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<bool> Logout(string phoneNumber, string sentCode, string code)
        {
            return await _queryDispatcher.DispatchAsync<Logout.Query, bool>(new Logout.Query()
            {
                UserId = _userProvider.UserId(User)
            });
        }

        [HttpPost]
        [Route("SendCode")]
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