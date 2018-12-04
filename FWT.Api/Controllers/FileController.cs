using FWT.Api.Controllers.File;
using FWT.Core.CQRS;
using FWT.Core.Helpers;
using FWT.Core.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenTl.Schema;
using System.Threading.Tasks;

namespace FWT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICurrentUserProvider _userProvider;

        public FileController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICurrentUserProvider userProvider)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _userProvider = userProvider;
        }

        [HttpGet]
        [Authorize]
        public async Task<FileContentResult> GetChats(long volumeId, int localId, long secret)
        {
            var result = await _queryDispatcher.DispatchAsync<GetFile.Query, FileInfo>(new GetFile.Query()
            {
                PhoneHashId = _userProvider.PhoneHashId(User),
                Location = new TInputFileLocation()
                {
                    LocalId = localId,
                    VolumeId = volumeId,
                    Secret = secret
                }
            });

            return File(result.Content, "image/jpeg", result.Name);
        }
    }
}