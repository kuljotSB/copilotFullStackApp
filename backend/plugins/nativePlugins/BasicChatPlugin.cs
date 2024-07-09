using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.SemanticKernel.TemplateEngine;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using Microsoft.SemanticKernel.Plugins.Web;
using Microsoft.Azure.Cosmos;
using Microsoft.SemanticKernel.Plugins.Web.Bing;
using Newtonsoft.Json;
using Kernel = Microsoft.SemanticKernel.Kernel;
using System.ComponentModel;
using backend.api.Credentials;
using backend.api.Dtos;

namespace backend.api.NativePlugins;

public class BasicChatPlugin
{
     [KernelFunction, Description("To have a basic chat with the AI assistant. Its as if the user was chatting with ChatGPT.")]
    public async Task<string> BasicChatFunction([Description("the user query to chat with the AI assistant")] string user_query)
    {
        OpenAIClient client = new OpenAIClient(new Uri(Secrets.openaiEndpoint!), new AzureKeyCredential(Secrets.openaiKey!));
        try{
           CosmosClient cosmosClient = new CosmosClient($"{Secrets.cosmosDBConnectionString}");
            Database database = cosmosClient.GetDatabase("chatHistoryDB");
            var databaseResponse = await database.ReadAsync();
            Microsoft.Azure.Cosmos.Container container = database.GetContainer("container-1");
            DateTime currentDate = DateTime.Now;
            string date = currentDate.ToString("yyyy-MM-dd");

             var query = new QueryDefinition("SELECT * FROM c WHERE c.Date = @date ORDER BY c._ts DESC")
                    .WithParameter("@date", $"{date}");

            
            
            using FeedIterator<ChatHistoryDB> feed = container.GetItemQueryIterator<ChatHistoryDB>(query);
            List<ChatCompletionsFormat> chatHistoryList = new List<ChatCompletionsFormat>();

            
            FeedResponse<ChatHistoryDB> chatItemResponse = await feed.ReadNextAsync();
            int count=0;
                // Iterate query results
          
            foreach (ChatHistoryDB item in chatItemResponse)
            {
                chatHistoryList.Insert(0, new ChatCompletionsFormat(item.UserQuery!, item.AssistantResponse!));
                count++;
                if (count >= 5)
                {
                    break;
                }
            }
            

            string systemMessage = "You are a helpful AI assistant meant to assist the user by answering their queries.";
            string userMessage = user_query;

            ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
            {
                Messages =
                {
                  new ChatRequestSystemMessage(systemMessage),  
                },
                MaxTokens = 400,
                Temperature = 0.7f,
                DeploymentName = Secrets.openaiChatModel
            };

            foreach (ChatCompletionsFormat item in chatHistoryList)
            {
              Console.WriteLine(item.userQuery);
                chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(item.userQuery));
                chatCompletionsOptions.Messages.Add(new ChatRequestAssistantMessage(item.assistantResponse));
            }

            chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(user_query));
            ChatCompletions response = client.GetChatCompletions(chatCompletionsOptions);

      // Print the response
      string chatResponse = response.Choices[0].Message.Content;
      return chatResponse;



     }
     catch(Exception error)
     {

      string systemMessage = "You are a helpful AI assistant meant to assist the user by answering their queries.";
      string userMessage = user_query;

      ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
      {
          Messages =
          {
              new ChatRequestSystemMessage(systemMessage),
              new ChatRequestUserMessage(userMessage),
          },
          MaxTokens = 400,
          Temperature = 0.7f,
          DeploymentName = Secrets.openaiChatModel
      };


      ChatCompletions response = client.GetChatCompletions(chatCompletionsOptions);

      // Print the response
      string chatResponse = response.Choices[0].Message.Content;
      return chatResponse;
     }
    }

}