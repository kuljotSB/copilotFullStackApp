using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure;
using Azure.Storage.Queues;
using Microsoft.Azure.Cosmos;   
using Newtonsoft.Json;
using Azure.Storage.Queues.Models;
using Function.Data;
namespace StorageQueueFunction
{
    public class QueueStorageFunction
    {
        private readonly ILogger<QueueStorageFunction> _logger;

        public QueueStorageFunction(ILogger<QueueStorageFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(QueueStorageFunction))]
        public async Task Run([QueueTrigger("chatqueue", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
            Console.WriteLine($"C# Queue trigger function processed: {message.MessageText}");

            QueueData data = JsonConvert.DeserializeObject<QueueData>(message.MessageText);

            CosmosClient cosmosClient = new CosmosClient($"{Secrets.cosmosDBConnectionString}");
           
            DatabaseResponse databaseResponse = await cosmosClient.CreateDatabaseIfNotExistsAsync("chatHistoryDB");

            Database database = cosmosClient.GetDatabase("chatHistoryDB");

            Container container1 = await database.CreateContainerIfNotExistsAsync(
                id: "container-1",
                partitionKeyPath: "/Date"
            );

            Container container = database.GetContainer("container-1");

            ChatHistoryDB chatHistoryDB = new ChatHistoryDB(data.Id, data.Date, data.UserQuery, data.AssistantResponse);
            
            ItemResponse<ChatHistoryDB> response = await container.UpsertItemAsync<ChatHistoryDB>(
                item: chatHistoryDB,
                partitionKey: new PartitionKey(chatHistoryDB.Date)
            );
            }

        }
    }

