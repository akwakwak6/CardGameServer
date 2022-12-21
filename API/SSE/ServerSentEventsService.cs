using API.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Runtime.Intrinsics.X86;

namespace API.SSE {


    public class ServerSentEventsService {

        private readonly ConcurrentDictionary<Guid, ServerSentEventsClient> _clients = new ConcurrentDictionary<Guid, ServerSentEventsClient>();

        public Guid AddClient(ServerSentEventsClient client) {
            Guid clientId = Guid.NewGuid();

            _clients.TryAdd(clientId, client);

            return clientId;
        }

        public int RemoveClient(Guid clientId) {
            ServerSentEventsClient client;

            _clients.TryRemove(clientId, out client);
            return _clients.Count;
        }

        public Task SendObjectAsync(Object o) {

            SseModel sse = new SseModel() {
                Type = o.GetType().Name,
                Data = new string[] { JsonConvert.SerializeObject(o) }
            };

            return SendEventAsync(sse);
        }

        public Task SendEventAsync(SseModel sse) {
            List<Task> clientsTasks = new List<Task>();
            foreach (ServerSentEventsClient client in _clients.Values) {
                clientsTasks.Add(client.SendEventAsync(sse));
            }
            return Task.WhenAll(clientsTasks);
        }

    }
}
