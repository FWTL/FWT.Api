using FluentValidation;
using FWT.TL.Core.Data;
using OpenTl.ClientApi;
using System.Threading.Tasks;
using System.Web.Http;

namespace FWT.TL.API.Controllers
{
    public class AuthController : ApiController
    {
        private IClientApi _client;
        private IUnitOfWork _unitOfWork;

        public AuthController(IClientApi client, IUnitOfWork unitOfWork)
        {
            _client = client;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task SendCode(string phoneNumber)
        {
            var result = await _client.AuthService.SendCodeAsync(phoneNumber);
        }
    }
}