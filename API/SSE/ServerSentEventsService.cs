using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace API.SSE {


    public class ServerSentEventsService {

        private readonly ConcurrentDictionary<Guid, ServerSentEventsClient> _clients = new ConcurrentDictionary<Guid, ServerSentEventsClient>();

        internal Guid AddClient(ServerSentEventsClient client) {
            Guid clientId = Guid.NewGuid();

            _clients.TryAdd(clientId, client);

            return clientId;
        }

        internal void RemoveClient(Guid clientId) {
            ServerSentEventsClient client;

            _clients.TryRemove(clientId, out client);
        }

        public Task SendObjectAsync(Object o) {
            Console.WriteLine(JsonConvert.SerializeObject(o));
            Console.WriteLine(o.GetType().ToString());

            ServerSentEvent sse = new ServerSentEvent() {
                Id = "42",
                Data = new string[] { JsonConvert.SerializeObject(o) }
            };

            return SendEventAsync(sse);
        }

        public Task SendEventAsync(ServerSentEvent serverSentEvent) {
            List<Task> clientsTasks = new List<Task>();
            foreach (ServerSentEventsClient client in _clients.Values) {
                clientsTasks.Add(client.SendEventAsync(serverSentEvent));
            }

            return Task.WhenAll(clientsTasks);
        }

    }
}
