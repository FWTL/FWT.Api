using System.Threading.Tasks;
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

            public Handler(ITelegramService telegramService, IGuidService guidService)
            {
                _telegramService = telegramService;
                _guidService = guidService;
            }

            public IGuidService _guidService { get; }

            public async Task<FileInfo> HandleAsync(Query query)
            {
                IClientApi client = await _telegramService.BuildAsync(query.PhoneHashId);
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

            public string PhoneHashId { get; set; }
        }
    }
}
