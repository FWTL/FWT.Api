using System.Threading.Tasks;
using FWT.Core.CQRS;
using FWT.Core.Helpers;
using FWT.Core.Services.Telegram;
using FWT.Core.Services.Unique;
using FWT.Infrastructure.Telegram;
using OpenTl.ClientApi;
using OpenTl.Schema;
using OpenTl.Schema.Contacts;

namespace FWT.Api.Controllers.File
{
    public class GetFile
    {
        public class Query : IQuery
        {
            public string PhoneHashId { get; set; }
            public TInputFileLocation Location { get; set; }
        }

        public class Handler : IQueryHandler<Query, FileInfo>
        {
            private readonly ITelegramService _telegramService;

            public IGuidService _guidService { get; }

            public Handler(ITelegramService telegramService, IGuidService guidService)
            {
                _telegramService = telegramService;
                _guidService = guidService;
            }

            public async Task<FileInfo> HandleAsync(Query query)
            {
                IClientApi client = await _telegramService.BuildAsync(query.PhoneHashId);
                byte[] result = (await TelegramRequest.Handle(() =>
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
    }
}