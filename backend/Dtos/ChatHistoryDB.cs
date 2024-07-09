namespace backend.api.Dtos;

public class ChatHistoryDB {
    public string? id { get; set; }
    public string? Date { get; set; }
    public string? UserQuery { get; set; }
    public string? AssistantResponse { get; set; }

    public ChatHistoryDB(string? Id, string? date, string? userQuery, string? assistantResponse)
    {
        id = Id;
        Date = date;
        UserQuery = userQuery;
        AssistantResponse = assistantResponse;
    }
}