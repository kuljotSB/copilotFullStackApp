using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;

namespace backend.api.Credentials;

public static class Secrets{
    public static IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    public static readonly string? openaiKey = config["OPENAI_KEY"];
    public static readonly string? openaiEndpoint = config["OPENAI_ENDPOINT"];
    public static readonly  string? openaiChatModel = config["OPENAI_CHAT_MODEL"];
    public static readonly string? bingApiKey = config["BING_API_KEY"];

    public static readonly string? AISearchKey = config["AI_SEARCH_KEY"];
    public static readonly string? AISearchEndpoint = config["AI_SEARCH_ENDPOINT"];
    public static readonly string? AISearchIndex = config["AI_SEARCH_INDEX_NAME"];
    public static readonly string? storageConnectionString = config["STORAGE_CONNECTION_STRING"];
    public static readonly string? cosmosDBConnectionString = config["COSMOSDB_CONNECTION_STRING"];
    public static readonly string? clientId = config["CLIENTID"];
    public static readonly string? tenantId = config["TENANTID"];
}