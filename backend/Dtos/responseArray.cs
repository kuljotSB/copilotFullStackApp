namespace backend.api.Dtos
{
    public class ResponseArray
    {
        public string? ChatResponse { get; set; }
        public string? SerializedPlan { get; set; }

       public ResponseArray(string chatResponse, string serializedPlan)
        {
            ChatResponse = chatResponse;
            SerializedPlan = serializedPlan;
        }
    }
}