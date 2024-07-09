using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TemplateEngine;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using Newtonsoft.Json;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using System.ComponentModel;
using backend.api.Credentials;
using Kernel = Microsoft.SemanticKernel.Kernel;

namespace backend.api.NativePlugins;

public class AISearchPlugin{
     [KernelFunction, Description("To Search for basic information related to documents that contain reviews about various hotels at various locations such as USA, UK, Britain, UAE etc. These documents are indexed via Azure AI Search thus building the foundation fro RAG architecture.")]
     public string BasicRAGChat([Description("the user query whose answer needs to be searched from the review documents indexed into Azure AI Search")] string userQuery)
     {
        SearchClient searchClient = new SearchClient(new Uri(Secrets.AISearchEndpoint!), Secrets.AISearchIndex, new AzureKeyCredential(Secrets.AISearchKey!));
        SearchResults<SearchDocument> searchResults = searchClient.Search<SearchDocument>($"{userQuery}");
        int count=0;
        string content="";
        string url="";
        foreach (SearchResult<SearchDocument> result in searchResults.GetResults())
        {
            if(count==0) {
            SearchDocument doc = result.Document;
            dynamic data = JsonConvert.DeserializeObject(doc.ToString())!;
            content = data.content;
            url=data.url;
            count=count+1;
            }
        }

        string systemMessage = @"you are a helpful AI Assistant that is meant to answer user queries.
        the user will ask questions regarding certain hotels and you need to answer the user query from the grounding content that will be provided to you.
        this grounding content is basically the review of the hotel that the user inquired to know about. This grounding 
        content has been taken after searching the Azure AI Search index that contains reviews about various hotels at various locations such as USA, UK, Britain, UAE etc.
        You will also be supplied with the url to the document that contains the grounding content. so make sure you also include the URL in the answer.
        the ending line could sound something like:
        For more information, you can visit the following link: <the-URL-goes-here>";

        string systemPrompt = $@"answer the user query based on the grounding content that is provided to you.
        the user query is : {userQuery}
        the grounding content is : {content}
        the URL to the document containing the grounding content is : {url}";

        OpenAIClient client = new OpenAIClient(new Uri(Secrets.openaiEndpoint!), new AzureKeyCredential(Secrets.openaiKey!));

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

        ChatCompletions finalResponse = client.GetChatCompletions(chatCompletionsOptions);
        string completion = finalResponse.Choices[0].Message.Content;

        return completion;

     }

     [KernelFunction, Description("To search for information related to the most commonly occuring keyphrases in the documents that contain reviews about various hotels at various locations such as USA, UK, Britain, UAE etc. These documents are indexed via Azure AI Search thus building the foundation fro RAG architecture.")]
     public string RAGKeyphrasesSearch([Description("the user query whose answer needs to be searched from the review documents indexed into Azure AI Search")] string userQuery)
     {
        SearchClient searchClient = new SearchClient(new Uri(Secrets.AISearchEndpoint!), Secrets.AISearchIndex, new AzureKeyCredential(Secrets.AISearchKey!));
        SearchResults<SearchDocument> searchResults = searchClient.Search<SearchDocument>($"{userQuery}");

        int count=0;
        string keyphrases="";
        string url="";

        foreach (SearchResult<SearchDocument> result in searchResults.GetResults())
        {
            if(count==0) {
            SearchDocument doc = result.Document;
            dynamic data = JsonConvert.DeserializeObject(doc.ToString())!;
            var keyPhrasesList = data.keyphrases;
            url = data.url;
            foreach (var keyphrase in keyPhrasesList)
            {
            keyphrases=keyphrases+keyphrase+",";
            }
            count=count+1;
            }
        }

        string systemMessage = @$"you are a helpful AI assistant that is meant to answer user queries. You will be provided
        with the keyphrases occuring in the grounding data. The grounding data is basically pdf documents containing reviews of various
        hotels in places like USA, UK, UAE, Britain etc. 
        Your answer needs to be built/generated on the following parameters:
        1)Your answer needs to include a list of all the important keyphrases that are present in the grounding data.
        2)By analysis the keyphrases, you need to provide a summary of the grounding data.
        You will also be provided with a URL to the PDF document of the grounding data, so make sure to give a reference to the URL as well
        at the end of your answer. The ending line could sound something like:
        For more information, you can visit the following link: <the-URL-goes-here>";

        string systemPrompt = $@"answer the user query based on the grounding data/context provided to you:
        the user query is : {userQuery}
        the keyphrases in the grounding data are : {keyphrases}
        the url is : {url}";

        OpenAIClient client = new OpenAIClient(new Uri(Secrets.openaiEndpoint!), new AzureKeyCredential(Secrets.openaiKey!));

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

        ChatCompletions finalResponse = client.GetChatCompletions(chatCompletionsOptions);
        string completion = finalResponse.Choices[0].Message.Content;

        return completion;



    }

    [KernelFunction, Description("To search for information related to the most commonly occuring locations in the documents that contain reviews about various hotels at various locations such as USA, UK, Britain, UAE etc. These documents are indexed via Azure AI Search thus building the foundation fro RAG architecture.")]
    public string RAGLocationsSearch([Description("the user query whose answer needs to be searched from the indexed documents")] string userQuery)
    {
       SearchClient searchClient = new SearchClient(new Uri(Secrets.AISearchEndpoint!), Secrets.AISearchIndex, new AzureKeyCredential(Secrets.AISearchKey!));
       SearchResults<SearchDocument> searchResults = searchClient.Search<SearchDocument>($"{userQuery}");
       int count=0;
       string locations = "";
       string url="";

       foreach (SearchResult<SearchDocument> result in searchResults.GetResults())
        {
            if(count==0) {
            SearchDocument doc = result.Document;
            dynamic data = JsonConvert.DeserializeObject(doc.ToString())!;
            var locationList = data.locations;
            foreach (var location in locationList)
            {
            locations=locations+location+",";
            }
            count=count+1;
            }
        }
       var systemMessage = @$"you are a helpful AI assistant that is meant to answer user queries. You will be provided
        with the locations occuring in the grounding data. The grounding data is basically pdf documents containing reviews of various
        hotels in places like USA, UK, UAE, Britain etc. 
        Your answer needs to be built/generated on the following parameters:
        1)Your answer needs to include a list of all the important locations that are present in the grounding data.
        2)By analysis of the locations, you need to provide a summary of the grounding data or maybe some important knowledge insight if you find any.
        You will also be provided with a URL to the PDF document of the grounding data, so make sure to give a reference to the URL as well
        at the end of your answer. The ending line could sound something like:
        For more information, you can visit the following link: <the-URL-goes-here>";

      string systemPrompt = @$"answer the user query based on the grounding data/context provided to you:
      the user query is : {userQuery}
      the locations in the grounding data are : {locations}
      the url is : {url}";

      OpenAIClient client = new OpenAIClient(new Uri(Secrets.openaiEndpoint!), new AzureKeyCredential(Secrets.openaiKey!));

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

      ChatCompletions finalResponse = client.GetChatCompletions(chatCompletionsOptions);
      string completion = finalResponse.Choices[0].Message.Content;

      return completion;
    }

    











   








}