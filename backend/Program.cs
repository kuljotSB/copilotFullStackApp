using backend.api.Dtos;
using backend.api.pluginFunctions;
using System.IO;
using Microsoft.Extensions.Configuration.Json;
using System.Text.Json;
using backend.api.StorageQueue;
using backend.api.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

    



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
 

var app = builder.Build();

app.UseCors(options => 
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});






app.MapPost("/Chat", async (UserQuery userQuery) => {
    string? userQueryString = userQuery.userQueryString;
    Boolean choice = userQuery.WebSearcherPluginChoice;
    Boolean choiceForAISearch = userQuery.AISearchPluginChoice;
    Boolean graphPluginChoice = userQuery.GraphPluginChoice;

    // Create an instance of the Program class
    var plugin = new Plugin();

    // Call the BasicChat method on the instance
    var responseArray = new ResponseArray(
        chatResponse: "",
        serializedPlan: ""
    );

    responseArray = await plugin.BasicChat(userQueryString!, choice, choiceForAISearch, graphPluginChoice);

    var chatResponseList =  new List<ChatResponse>
    {
        new ChatResponse
        {
            chatResponse = responseArray.ChatResponse,
            serializedPlan = responseArray.SerializedPlan
        }
    };

    Guid newGuid = Guid.NewGuid();
    string stringGuid=newGuid.ToString();

    DateTime currentDate = DateTime.Now;

    QueueData queueData = new QueueData(
        stringGuid,
        currentDate.ToString("yyyy-MM-dd"),
        userQueryString!,
        responseArray.ChatResponse!

    );
    
    StorageQueue storageQueue = new StorageQueue();
    await storageQueue.sendMessageToQueue(queueData);

   

    
    
     

    // Return the chat response
    return Results.Ok(chatResponseList);
});

app.MapGet("/acquireaccesstoken", ()=>{
    var authentication = new Authentication();
    var token =  authentication.GetAccessToken();
    TokenManager.AccessToken = token;
    return Results.Ok(token);

});

app.Run();






