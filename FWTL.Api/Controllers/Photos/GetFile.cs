﻿using System.Threading.Tasks;
using FWTL.Core.CQRS;
using FWTL.Core.Helpers;
using FWTL.Core.Services.Telegram;
using FWTL.Core.Services.Unique;
using FWTL.Infrastructure.Telegram;
using OpenTl.ClientApi;
using OpenTl.Schema;

namespace FWTL.Api.Controllers.Photos
{
    public class GetPhoto
    {
        public class Handler : IQueryHandler<Query, FileInfo>
        {
            private readonly ITelegramService _telegramService;
            private readonly IGuidService _guidService;

            public Handler(ITelegramService telegramService, IGuidService guidService)
            {
                _telegramService = telegramService;
                _guidService = guidService;
            }

            public async Task<FileInfo> HandleAsync(Query query)
            {
                IClientApi client = await _telegramService.BuildAsync(query.UserId);
                byte[] result = (await TelegramRequest.HandleAsync(() =>
                {
                    return client.FileService.DownloadFullFileAsync(query.Location);
                }));

                return new FileInfo()
                {
                    Content = result,
                    Name = _guidService.New().ToString("n")
                };
            }
        }

        public class Query : IQuery
        {
            public TInputFileLocation Location { get; set; }

            public string UserId { get; set; }
        }
    }
}