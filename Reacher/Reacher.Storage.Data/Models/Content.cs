namespace Reacher.Storage.Data.Models
{
    public class Content
    {
        public Content() { }

        public Content(string id, string message)
        {
            Id = id;
            Message = message;
        }

        public string Id { get; set; }
        public string Message { get; set; }

        public override string ToString()
            => $"Content - 'Id: {Id}, Message: {Message}'";

        public bool IsProvided
            => !string.IsNullOrWhiteSpace(Id) && !string.IsNullOrWhiteSpace(Message);
    }
}