using System.Collections.Generic;
using System.Threading.Tasks;
using FWTL.Api.Controllers.Chat;
using FWTL.Core.CQRS;
using FWTL.Core.Services.User;
using FWTL.Infrastructure.Telegram.Parsers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FWTL.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        private readonly IQueryDispatcher _queryDispatcher;

        private readonly ICurrentUserProvider _userProvider;

        public ChannelsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICurrentUserProvider userProvider)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _userProvider = userProvider;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<Channel>> GetChannels()
        {
            return await _queryDispatcher.DispatchAsync<GetChannels.Query, List<Channel>>(new GetChannels.Query()
            {
                PhoneHashId = _userProvider.PhoneHashId(User)
            });
        }
    }
}
