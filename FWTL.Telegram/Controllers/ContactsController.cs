using System.Collections.Generic;
using System.Threading.Tasks;
using FWTL.Core.CQRS;
using FWTL.Core.Services.User;
using FWTL.Events.Telegram.Messages;
using FWTL.Telegram.Controllers.Contacts;
using FWTL.Telegram.Controllers.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FWTL.Events.Telegram.Enums;

namespace FWTL.Telegram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        private readonly IQueryDispatcher _queryDispatcher;

        private readonly ICurrentUserProvider _userProvider;

        public ContactsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICurrentUserProvider userProvider)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _userProvider = userProvider;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<Contact>> GetContacts()
        {
            return await _queryDispatcher.DispatchAsync<GetContacts.Query, List<Contact>>(new GetContacts.Query()
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
                Type = PeerType.User
            });
        }
    }
}