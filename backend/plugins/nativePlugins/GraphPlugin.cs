using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.SemanticKernel.TemplateEngine;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Linq;
using Microsoft.SemanticKernel.Plugins.Web;
using Microsoft.Azure.Cosmos;
using Microsoft.SemanticKernel.Plugins.Web.Bing;
using Newtonsoft.Json;
using Kernel = Microsoft.SemanticKernel.Kernel;
using System.ComponentModel;
using backend.api.Credentials;
using backend.api.Dtos;
using backend.api.Authentication;
using System.Text;

namespace backend.api.NativePlugins;

public class GraphPlugin
{
    [KernelFunction, Description("To list the calendar events of the user such as meetings etc.")]
    public async Task<string> ListCalendarEvents([Description("the query of the user")]string userQuery)
    {
        string url = "https://graph.microsoft.com/v1.0/me/events?$select=subject,body,bodyPreview,organizer,attendees,start,end,location";
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {TokenManager.AccessToken}");
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage response = await client.GetAsync(url);
        string responseString = await response.Content.ReadAsStringAsync();
        
        Console.WriteLine(responseString);

        OpenAIClient openAIClient = new OpenAIClient(new Uri(Secrets.openaiEndpoint!), new AzureKeyCredential(Secrets.openaiKey!));

        string systemMessage = @$"You are a helpful AI assistant meant to assist the user by answering their queries related to knowing the calendar events in
        the microsoft graph API. you will be presented with the user query that the user asked and a JSON response of the graph API. Extract
        information from that JSON response based on the user query and present it to the user in a readable format.";

        string systemPrompt = @$"The user query is: {userQuery}. The JSON response from the graph API is: {responseString}. 
        Extract information from the JSON response based on the user query and present it to the user in a readable format.";

        
        ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
        {
            Messages =
            {
                new ChatRequestSystemMessage(systemMessage),
                new ChatRequestUserMessage(systemPrompt),
            },
            MaxTokens = 400,
            Temperature = 0.7f,
            DeploymentName = Secrets.openaiChatModel
        };

    ChatCompletions finalResponse = openAIClient.GetChatCompletions(chatCompletionsOptions);
    string completion = finalResponse.Choices[0].Message.Content;

      return completion;


    }

    [KernelFunction, Description("to get the information of the mail inbox of the user using the Microsoft Graph API. queries could include something like 'what was the last mail i received?' etc")]
    public async Task<string> GetMailList([Description("the query of the user")] string userQuery)
    {
        string url = "https://graph.microsoft.com/v1.0/me/messages";

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {TokenManager.AccessToken}");
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage response = await client.GetAsync(url);
        string responseString = await response.Content.ReadAsStringAsync();
        
        dynamic data = JsonConvert.DeserializeObject(responseString)!;
        // Convert JArray to List<dynamic> before using Take

        var dataValue = data["value"];
        var topFiveItems = new List<dynamic>();

// Iterate through the first five items and add them to the list
        for (int i = 0; i < 2 && i < dataValue.Count; i++)
        {
            topFiveItems.Add(dataValue[i]);
        }
       
       string topFiveItemsString = JsonConvert.SerializeObject(topFiveItems);
        
        

        

        OpenAIClient openAIClient = new OpenAIClient(new Uri(Secrets.openaiEndpoint!), new AzureKeyCredential(Secrets.openaiKey!));

       string systemMessage = @$"You are a helpful AI assistant meant to assist the user by answering their queries related to knowing the mail messages in
        the microsoft graph API. you will be presented with the user query that the user asked and a JSON response of the graph API. Extract
        information from that JSON response based on the user query and present it to the user in a readable format.";

        string systemPrompt = @$"The user query is: {userQuery}. The JSON response from the graph API is: {topFiveItemsString}. 
        Extract information from the JSON response based on the user query and present it to the user in a readable format.";

        
        ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
        {
            Messages =
            {
                new ChatRequestSystemMessage(systemMessage),
                new ChatRequestUserMessage(systemPrompt),
            },
            MaxTokens = 400,
            Temperature = 0.7f,
            DeploymentName = Secrets.openaiChatModel
        };
       try {
       ChatCompletions finalResponse = openAIClient.GetChatCompletions(chatCompletionsOptions);
       string completion = finalResponse.Choices[0].Message.Content;

      return completion;
       }
       catch(Exception error)
       {
        Console.WriteLine(error);
           return error.Message;
       }
    }

    [KernelFunction, Description("To send an email to a user using the Microsoft Graph API")]
    public async Task<string> sendMail([Description("the query of the user")] string userQuery)
    {
        try{
       OpenAIClient openAIClient = new OpenAIClient(new Uri(Secrets.openaiEndpoint!), new AzureKeyCredential(Secrets.openaiKey!));

       string firstSystemMessage = @$"You are a helpful AI assistant. The thing is the user wants to send an email to someone
       using the Microsoft Graph API. You will be provided with the uer query and based upon the user query you would need to
       compose the body content of the mail. A sample would look something like this:
       userQuer: compose an email to kuljot.bakshi302005@gmail.com congratulating him on his new milestone.
       
       Assistant Response: Congratulations Kuljot!!!! may you achieve more milestones like this!!!";

       string firstSystemPrompt = @$"The User query is : {userQuery} ";

       ChatCompletionsOptions chatCompletionsOptions = new ChatCompletionsOptions()
        {
            Messages =
            {
                new ChatRequestSystemMessage(firstSystemMessage),
                new ChatRequestUserMessage(firstSystemPrompt),
            },
            MaxTokens = 400,
            Temperature = 0.7f,
            DeploymentName = Secrets.openaiChatModel
        };

        ChatCompletions finalResponse = openAIClient.GetChatCompletions(chatCompletionsOptions);
        string composedEmailBody = finalResponse.Choices[0].Message.Content;
        Console.WriteLine(composedEmailBody);
        var builder = Kernel.CreateBuilder();

        builder.AddAzureOpenAIChatCompletion(Secrets.openaiChatModel!, Secrets.openaiEndpoint!, Secrets.openaiKey!);

        var kernel = builder.Build();

        string ParentDirectory = Directory.GetCurrentDirectory();
        string WriterPluginPath = Path.Combine(ParentDirectory, "plugins", "promptTemplatePlugins", "WriterPlugin");

        var writerPlugin = kernel.ImportPluginFromPromptDirectory(WriterPluginPath);

        var extractEmailPlugin = writerPlugin["extractEmail"];

        var arguments = new KernelArguments() {
            ["input"] = userQuery
        };

        string emailAd = (await kernel.InvokeAsync(extractEmailPlugin, arguments)).ToString();

        Console.WriteLine(emailAd);

        string draftMessageUrl = "https://graph.microsoft.com/v1.0/me/messages";

        HttpClient httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {TokenManager.AccessToken}");

         var data = new
            {
                subject = "Message from copilot",
                importance = "Low",
                body = new
                {
                    contentType = "HTML",
                    content = $"{composedEmailBody}"
                },
                toRecipients = new[]
                {
                    new
                    {
                        emailAddress = new
                        {
                            address = emailAd
                        }
                    }
                }
            };

            var jsonContent = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json"
            );

            var emailDraftMessage = await httpClient.PostAsync(draftMessageUrl, jsonContent);

            dynamic emailDraftMessageData = JsonConvert.DeserializeObject(await emailDraftMessage.Content.ReadAsStringAsync())!;

            string emailDraftMessageId = emailDraftMessageData.id;

            string finalURL = $"https://graph.microsoft.com/v1.0/me/messages/{emailDraftMessageId}/send";

            var sendEmail = await httpClient.PostAsync(finalURL, null);

            return "successfully sent message";
        }
        catch(Exception error)
        {
            Console.WriteLine(error);
            return error.Message;
        }

      
      



        


    }



        


    
}
