using FWT.TL.Core;
using FWT.TL.Core.Services.Telegram;
using OpenTl.ClientApi;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FWT.TL.Infrastructure.Telegram
{
    public class UserSessionManager : IUserSessionManager
    {
        private Dictionary<string, IClientApi> _clients = new Dictionary<string, IClientApi>();

        public UserSessionManager()
        {
        }

        public async Task<IClientApi> Get(string key, ISessionStore store)
        {
            if (_clients.ContainsKey(key))
            {
                return _clients[key];
            }

            var settings = new FactorySettings
            {
                AppHash = ConfigKeys.TelegramApiHash,
                AppId = ConfigKeys.TelegramApiId,
                //ServerAddress = "149.154.167.50",
                ServerAddress = "149.154.175.10",
                ServerPublicKey = ConfigKeys.RsaPublicKey,
                ServerPort = 443,
                SessionTag = key,
                Properties = new ApplicationProperties
                {
                    AppVersion = "1.0.0",
                    DeviceModel = "PC",
                    LangCode = "en",
                    LangPack = "tdesktop",
                    SystemLangCode = "en",
                    SystemVersion = "Win 10 Pro"
                },
                SessionStore = new FakeSessionStore()
            };

            _clients[key] = await ClientFactory.BuildClientAsync(settings);
            return _clients[key];
        }
    }
}