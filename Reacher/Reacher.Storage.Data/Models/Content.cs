namespace Reacher.Storage.Data.Models
{
    public class Content
    {
        public Content(string id, string message)
        {
            Id = id;
            Message = message;
        }

        public string Id { get; private set; }
        public string Message { get; private set; }

        public override string ToString()
            => $"Content - 'Id: {Id}, Message: {Message}'";
    }
}