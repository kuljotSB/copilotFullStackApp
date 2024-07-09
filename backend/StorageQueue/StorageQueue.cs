using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using backend.api.Dtos;
using Microsoft.AspNetCore.DataProtection;
using backend.api.Credentials;
using System.Net;

namespace backend.api.StorageQueue;

public class StorageQueue
{


public async Task sendMessageToQueue(QueueData queueData)
{
    //setting the storage queue name
    string queueName = "chatqueue";

    //instantiating the queue client
    
    QueueClient queueClient = new QueueClient($"{Secrets.storageConnectionString!}", queueName, new QueueClientOptions{
        MessageEncoding = QueueMessageEncoding.Base64
    
    });
    
    //checking if the queue exists
    bool queueExists = await queueClient.ExistsAsync();

    if(!queueExists)
    {
        await queueClient.CreateAsync();
    }
    

    string Id = queueData.Id!;
    string Date = queueData.Date!;
    string UserQuery = queueData.UserQuery!;
    string AssistantResponse = queueData.AssistantResponse!;
    
    //setting the queue message in form of a string (Serializable JSON object)
    string message = @$" {{
        ""Id"": ""{Id}"",
        ""Date"": ""{Date}"",
        ""UserQuery"": ""{UserQuery}"",
        ""AssistantResponse"": ""{AssistantResponse}""
    }}";

    //sending the message to the queue
    await queueClient.SendMessageAsync(message);



}
}
