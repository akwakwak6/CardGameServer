using API.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace API.SSE {

    public class ServerSentEventsClient {

        private readonly HttpResponse _response;

        internal ServerSentEventsClient(HttpResponse response) {
            _response = response;
        }

        internal Task SendObjectAsync(Object o) {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return SendEventAsync(new SseModel() {
                Type = o.GetType().Name,
                Data = new string[] { JsonConvert.SerializeObject(o, serializerSettings) }
            });
        }

        internal Task SendEventAsync(SseModel sse) {
            return _response.WriteSseEventAsync(sse);
        }

    }
}
