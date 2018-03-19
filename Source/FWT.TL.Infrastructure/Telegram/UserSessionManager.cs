using FWT.TL.Core;
using FWT.TL.Core.Services.Telegram;
using OpenTl.ClientApi;
using OpenTl.ClientApi.MtProto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWT.TL.Infrastructure.Telegram
{
    public class UserSessionManager  : IUserSessionManager
    {

        private Dictionary<string, IClientApi> _clients = new Dictionary<string, IClientApi>();

        public UserSessionManager()
        {
           
        }

        public async Task<IClientApi> Get(string key)
        {
            if (_clients.ContainsKey(key))
            {
                return _clients[key];
            }

            var settings = new FactorySettings
            {
                AppHash = ConfigKeys.TelegramApiHash,
                AppId = ConfigKeys.TelegramApiId,
                ServerAddress = "149.154.167.50",
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
            };

            _clients[key] = await ClientFactory.BuildClient(settings).ConfigureAwait(false);
            return _clients[key];
        }
    }
}
