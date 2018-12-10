using FWT.Core.Services.CosmosDb;
using FWT.Infrastructure.Telegram.Parsers.Models;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;

namespace FWT.Infrastructure.CosmosDb
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly DocumentClient _client;
        private const string DB_NAME = "Messages";

        public CosmosDbService(DocumentClient client)
        {
            _client = client;
        }

        public async Task AddMessage(Message message)
        {
            await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DB_NAME, message.SourceId), message);
        }
    }
}