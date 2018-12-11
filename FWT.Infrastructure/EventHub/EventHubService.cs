using FWT.Core.Services.EventHub;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FWT.Infrastructure.EventHub
{
    public class EventHubService : IEventHubService
    {
        private readonly EventHubClient _client;

        public EventHubService(EventHubClient client)
        {
            _client = client;
        }

        public async Task SendAsync<TMessage>(List<TMessage> messages)
        {
            if (messages.Count > 0)
            {
                var batch = new EventDataBatch(100000);
                foreach (var msg in messages)
                {
                    batch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg))));
                }

                await _client.SendAsync(batch);
            }
        }
    }
}