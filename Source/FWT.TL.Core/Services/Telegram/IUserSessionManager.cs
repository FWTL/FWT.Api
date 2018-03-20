using OpenTl.ClientApi;
using System.Threading.Tasks;

namespace FWT.TL.Core.Services.Telegram
{
    public interface IUserSessionManager
    {
        Task<IClientApi> Get(string key, ISessionStore store);
    }
}