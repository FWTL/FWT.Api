using FWT.TL.Core.Data;
using FWT.TL.Core.Entities;
using FWT.TL.Core.Extensions;
using OpenTl.ClientApi;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FWT.TL.Infrastructure.Telegram
{
    public class FakeSessionStore : ISessionStore
    {
        public void Dispose()
        {
           
        }

        public byte[] Load()
        {
            return null;
        }

        public Task Save(byte[] session)
        {
            return Task.CompletedTask;
        }

        public void SetSessionTag(string sessionTag)
        {
           
        }
    }

    public class SqlSessionStore : ISessionStore
    {
        private IUnitOfWork _unitOfWork;
        private string _sessionKey;

        public SqlSessionStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
        }

        public byte[] Load()
        {
            if (!_sessionKey.ToN<int>().HasValue)
            {
                return null;
            }

            TelegramSession telegramSession = _unitOfWork.TelegramSessionRepository.GetSingle(_sessionKey.To<int>());
            if (telegramSession != null)
            {
                return telegramSession.Session;
            }

            return null;
        }

        public async Task Save(byte[] session)
        {
            var userId = _sessionKey.ToN<int>();

            if (!userId.HasValue)
            {
                return;
            }

            var userSession = await _unitOfWork.TelegramSessionRepository.Query().Where(ts => ts.Id == userId).FirstOrDefaultAsync();
            if (userSession.IsNotNull())
            {
                userSession.Session = session;
                _unitOfWork.TelegramSessionRepository.Update(userSession);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                userSession = new TelegramSession()
                {
                    Id = userId.Value,
                    Session = session,
                };

                _unitOfWork.TelegramSessionRepository.Insert(userSession);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public void SetSessionTag(string sessionKey)
        {
            _sessionKey = sessionKey;
        }
    }
}