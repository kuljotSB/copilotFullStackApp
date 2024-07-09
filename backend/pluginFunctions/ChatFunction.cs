using Azure;
using Azure.AI.OpenAI;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Web;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.SemanticKernel.Plugins.Web.Bing;
using Microsoft.SemanticKernel.TemplateEngine;
using Newtonsoft.Json;
using Kernel = Microsoft.SemanticKernel.Kernel;
using Microsoft.SemanticKernel.Planning.Handlebars;
using backend.api.Dtos;
using Microsoft.Azure.Cosmos;
using backend.api.NativePlugins;
using backend.api.Credentials;
namespace backend.api.pluginFunctions
{
  public class Plugin
  {
    public async Task<ResponseArray> BasicChat(string user_query, Boolean WebSearcherPluginChoice, Boolean AISearchPluginChoice, Boolean GraphPluginChoice)
    {
      IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
      string? key = config["OPENAI_KEY"];
      string? endpoint = config["OPENAI_ENDPOINT"];
      string? model = config["OPENAI_CHAT_MODEL"];

      

      var builder = Kernel.CreateBuilder();

      builder.AddAzureOpenAIChatCompletion(model!, endpoint!, key!);

      var kernel = builder.Build();

      string ParentDirectory = Directory.GetCurrentDirectory();


      
      string WriterPluginPath = Path.Combine(ParentDirectory, "plugins", "promptTemplatePlugins", "WriterPlugin");
      var writerPlugin = kernel.ImportPluginFromPromptDirectory(WriterPluginPath);
      
      var BasicChatPlugin = kernel.ImportPluginFromType<BasicChatPlugin>();
      
      
      if(WebSearcherPluginChoice == true)
      {
      var webSearcherPlugin = kernel.ImportPluginFromType<WebSearcherPlugin>();
      }
      if(AISearchPluginChoice == true)
      {
      var AISearchPlugin = kernel.ImportPluginFromType<AISearchPlugin>();
      }
      if(GraphPluginChoice == true)
      { 

      var graphPlugin = kernel.ImportPluginFromType<GraphPlugin>();
      }
      


      var planner = new HandlebarsPlanner(new HandlebarsPlannerOptions() { AllowLoops = true });

      try{
       var plan = await planner.CreatePlanAsync(kernel, user_query);

       var serializedPlan = plan.ToString();

       

       var result = await plan.InvokeAsync(kernel);

       var chatResponse = result.ToString();

       var stringSerializedPlan = serializedPlan.ToString();

      var responseArray = new ResponseArray(chatResponse, stringSerializedPlan);

      return responseArray;
      

    }
    catch(Exception e)
    {
      OpenAIClient client = new OpenAIClient(new Uri(Secrets.openaiEndpoint!), new AzureKeyCredential(Secrets.openaiKey!));
      Console.WriteLine(e);
      
     
 
     try{
      CosmosClient cosmosClient = new CosmosClient($"{Secrets.cosmosDBConnectionString}");
            Database database = cosmosClient.GetDatabase("chatHistoryDB");
            var databaseResponse = await database.ReadAsync();
            Container container = database.GetContainer("container-1");
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
      string stringSerializedPlan = "Made use of the default Chat Completions API";

      var responseArray = new ResponseArray(chatResponse, stringSerializedPlan);

     return responseArray;



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
      string stringSerializedPlan = "Made use of the default Chat Completions API";

      var responseArray = new ResponseArray(chatResponse, stringSerializedPlan);

     return responseArray;
     }



    }

    
  }
}
}