using API.Models;

namespace API.SSE {

    public class ServerSentEventsClient {

        private readonly HttpResponse _response;

        internal ServerSentEventsClient(HttpResponse response) {
            _response = response;
        }

        internal Task SendEventAsync(SseModel sse) {
            return _response.WriteSseEventAsync(sse);
        }

    }
}
