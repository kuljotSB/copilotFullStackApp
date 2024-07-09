namespace backend.api.Dtos;

public class ChatCompletionsFormat{
    public string? userQuery {get;set;}
    public string? assistantResponse {get; set;}


    public ChatCompletionsFormat(string userQuery, string assistantResponse)
    {
        this.userQuery = userQuery;
        this.assistantResponse = assistantResponse;
    }
}