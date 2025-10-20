using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using SupportApp.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SupportApp.Services
{
    public class SupportService : ISupportService
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public SupportService(IConfiguration config)
        {
            var connectionString = config["CosmosDb:ConnectionString"];
            var databaseId = config["CosmosDb:DatabaseId"];
            var containerId = config["CosmosDb:ContainerId"];

            _client = new CosmosClient(connectionString);
            _container = _client.GetContainer(databaseId, containerId); // v3 måde at få container på
        }

        public async Task AddSupportMessageAsync(SupportMessage message)
        {
            await _container.CreateItemAsync(message, new PartitionKey(message.PartitionKey));
        }

        public async Task<IEnumerable<SupportMessage>> GetAllSupportMessagesAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c ORDER BY c.CreatedAt DESC");
            var iterator = _container.GetItemQueryIterator<SupportMessage>(query);
            var results = new List<SupportMessage>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response.Resource);
            }

            return results;
        }
    }
}
