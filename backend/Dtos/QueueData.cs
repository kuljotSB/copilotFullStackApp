namespace backend.api.Dtos;

public class QueueData{
    public string? Id {get;set;}
    public string? Date {get;set;}
    public string? UserQuery {get;set;}
    public string? AssistantResponse {get;set;}

    public QueueData(string Id, string Date, String UserQuery, string AssistantResponse) {
        this.Id = Id;
        this.Date = Date;
        this.UserQuery = UserQuery;
        this.AssistantResponse = AssistantResponse;
    } 
}