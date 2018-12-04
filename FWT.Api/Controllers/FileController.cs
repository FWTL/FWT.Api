using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FWT.Api.Controllers.File;
using FWT.Core.CQRS;
using FWT.Core.Helpers;
using FWT.Core.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FWT.Api.Controllers
{
    public class FileController : Controller
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
        public async Task<FileInfo> GetChats()
        {
            return await _queryDispatcher.DispatchAsync<GetFile.Query, FileInfo>(new GetFile.Query()
            {
                PhoneHashId = _userProvider.PhoneHashId(User)
            });
        }
    }
}