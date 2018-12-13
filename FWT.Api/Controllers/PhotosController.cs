﻿using System.Threading.Tasks;
using FWTL.Api.Controllers.Photo;
using FWTL.Core.CQRS;
using FWTL.Core.Helpers;
using FWTL.Core.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenTl.Schema;

namespace FWTL.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;

        private readonly IQueryDispatcher _queryDispatcher;

        private readonly ICurrentUserProvider _userProvider;

        public PhotosController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICurrentUserProvider userProvider)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _userProvider = userProvider;
        }

        [HttpGet]
        [Authorize]
        public async Task<FileContentResult> GetChats(long volumeId, int localId, long secret)
        {
            var result = await _queryDispatcher.DispatchAsync<GetPhoto.Query, FileInfo>(new GetPhoto.Query()
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
