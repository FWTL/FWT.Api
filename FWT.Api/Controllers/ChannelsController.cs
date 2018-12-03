using FWT.Api.Controllers.Chat;
using FWT.Api.Controllers.User;
using FWT.Core.CQRS;
using FWT.Core.Services.User;
using FWT.Infrastructure.Telegram.Parsers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FWT.Api.Controllers
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
        public async Task<List<Channel>> GetChats()
        {
            return await _queryDispatcher.DispatchAsync<GetChannels.Query, List<Channel>>(new GetChannels.Query()
            {
                PhoneHashId = _userProvider.PhoneHashId(User)
            });
        }
    }
}