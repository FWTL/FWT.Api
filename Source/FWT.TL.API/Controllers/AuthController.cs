using FWT.TL.Core.Helpers;
using FWT.TL.Core.Services.Telegram;
using OpenTl.Schema;
using OpenTl.Schema.Auth;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace FWT.TL.API.Controllers
{
    public class AuthController : ApiController
    {
        private IUserSessionManager _sessionManager;
        private IDatabase _cache;

        public AuthController(IUserSessionManager sessionManager, IDatabase cache)
        {
            _sessionManager = sessionManager;
            _cache = cache;
        }

        [HttpPost]
        [Route("api/SendCode")]
        public async Task SendCode(string phoneNumber)
        {
            var client = await _sessionManager.Get(HashHelper.GetHash(phoneNumber), null);
            var result = await client.AuthService.SendCodeAsync(phoneNumber);

            if (result.PhoneRegistered)
            {
                await _cache.StringSetAsync($"TelegramCode.{HashHelper.GetHash(phoneNumber)}", result.PhoneCodeHash, TimeSpan.FromMinutes(5));
            }
        }

        [HttpPost]
        [Route("api/SignIn")]
        public async Task<TUser> SignIn(string phoneNumber, string code)
        {
            var phoneCodeHash = await _cache.StringGetAsync($"TelegramCode.{HashHelper.GetHash(phoneNumber)}");
            if (phoneCodeHash.IsNullOrEmpty)
            {
                return null;
            }

            var client = await _sessionManager.Get(HashHelper.GetHash(phoneNumber), null);
            var result = await client.AuthService.SignInAsync(phoneNumber, new TSentCode() { PhoneCodeHash = phoneCodeHash }, phoneCodeHash);
            return result;
        }
    }
}