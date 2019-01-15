using FWTL.Api.Controllers.Channels;
using FWTL.Core.CQRS;
using FWTL.Core.Services.User;
using FWTL.Events.Telegram.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                UserId = _userProvider.UserId(User)
            });
        }
    }
}