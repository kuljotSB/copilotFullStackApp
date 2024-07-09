namespace backend.api.Dtos;

public class WebSearcherResponse{
    
    public string answer {get; set;}

    public WebSearcherResponse(string answer){
        this.answer = answer;
    }
}