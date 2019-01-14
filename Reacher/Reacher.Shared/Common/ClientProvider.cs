namespace Reacher.Shared.Common
{
    public class ClientProvider : IClientProvider
    {
        private readonly string _clientId;

        public ClientProvider(string clientId)
        {
            _clientId = clientId;
        }

        public string ClientId() => _clientId.ToLowerInvariant();
    }
}