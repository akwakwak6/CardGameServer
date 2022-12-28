using API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        public SseModel getSSE(Object o) {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return new SseModel() {
                Type = o.GetType().Name,
                Data = new string[] { JsonConvert.SerializeObject(o, serializerSettings) }
            };
        }

        public async Task SendObjectAsync(Object o) {

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            SseModel sse = new SseModel() {
                Type = o.GetType().Name,
                Data = new string[] { JsonConvert.SerializeObject(o, serializerSettings) }
            };

            await SendEventAsync(sse);
        }

        public async Task SendEventAsync(SseModel sse) {
            


            //List<Task> clientsTasks = new List<Task>();
            foreach (ServerSentEventsClient client in _clients.Values) {
                //clientsTasks.Add(client.SendEventAsync(sse));
                //await client.SendEventAsync(sse);
            }
            //return Task.WhenAll(clientsTasks);
        }

    }
}
