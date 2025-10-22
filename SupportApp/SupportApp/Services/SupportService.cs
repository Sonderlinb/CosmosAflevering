using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using SupportApp.Models;
using System;
using System.Collections.Generic;
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

            Console.WriteLine($" Cosmos config loaded: DB={databaseId}, Container={containerId}");

            _client = new CosmosClient(connectionString);
            _container = _client.GetContainer(databaseId, containerId);
        }

        public async Task AddSupportMessageAsync(SupportMessage message)
        {
            message.Id ??= Guid.NewGuid().ToString();
            message.CreatedAt = DateTime.UtcNow;

            if (string.IsNullOrWhiteSpace(message.Category))
                message.Category = "Ukendt kategori";

            await _container.CreateItemAsync(message, new PartitionKey(message.Category));
        }

        public async Task<IEnumerable<SupportMessage>> GetAllSupportMessagesAsync()
        {
            var query = new QueryDefinition("SELECT * FROM c ORDER BY c.date DESC");
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
