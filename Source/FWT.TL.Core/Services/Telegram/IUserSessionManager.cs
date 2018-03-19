using OpenTl.ClientApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWT.TL.Core.Services.Telegram
{
    public interface IUserSessionManager
    {
        IClientApi Get(string key);
        IClientApi GetAnonymous();
    }
}
