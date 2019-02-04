using System.Collections.Generic;
using System.Threading.Tasks;
using FWTL.Telegram.Controllers.Chats;
using FWTL.Telegram.Controllers.Messages;
using FWTL.Core.CQRS;
using FWTL.Core.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FWTL.Core.Helpers.Enum;

namespace FWTL.Telegram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        private readonly IQueryDispatcher _queryDispatcher;

        private readonly ICurrentUserProvider _userProvider;

        public ChatsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICurrentUserProvider userProvider)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _userProvider = userProvider;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<Infrastructure.Telegram.Parsers.Models.Chat>> GetChats()
        {
            return await _queryDispatcher.DispatchAsync<GetChats.Query, List<Infrastructure.Telegram.Parsers.Models.Chat>>(new GetChats.Query()
            {
                UserId = _userProvider.UserId(User)
            });
        }

        [HttpPost]
        [Authorize]
        [Route("Process/{id}")]
        public async Task Process(int id)
        {
            await _commandDispatcher.DispatchAsync(new Process.Command()
            {
                Id = id,
                UserId = _userProvider.UserId(User),
                Type = PeerType.Chat
            });
        }
    }
}