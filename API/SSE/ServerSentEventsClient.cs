namespace API.SSE {

    public class ServerSentEvent {
        public string Id { get; set; }

        public string Type { get; set; }

        public IList<string> Data { get; set; }
    }


    internal static class ServerSentEventsHelper {
        internal static async Task WriteSseEventAsync(this HttpResponse response, ServerSentEvent serverSentEvent) {
            if (!String.IsNullOrWhiteSpace(serverSentEvent.Id))
                await response.WriteSseEventFieldAsync("id", serverSentEvent.Id);

            if (!String.IsNullOrWhiteSpace(serverSentEvent.Type))
                await response.WriteSseEventFieldAsync("event", serverSentEvent.Type);

            if (serverSentEvent.Data != null) {
                foreach (string data in serverSentEvent.Data)
                    await response.WriteSseEventFieldAsync("data", data);
            }

            await response.WriteSseEventBoundaryAsync();
            response.Body.Flush();
        }

        private static Task WriteSseEventFieldAsync(this HttpResponse response, string field, string data) {
            return response.WriteAsync($"{field}: {data}\n");
        }

        private static Task WriteSseEventBoundaryAsync(this HttpResponse response) {
            return response.WriteAsync("\n");
        }
    }


    public class ServerSentEventsClient {

        private readonly HttpResponse _response;

        internal ServerSentEventsClient(HttpResponse response) {
            _response = response;
        }

        public Task SendEventAsync(ServerSentEvent serverSentEvent) {
            /*//return _response.WriteSseEventAsync(serverSentEvent);
            await _response.WriteAsync("data: " + "super data" + "\n");
            //await _response.WriteAsync("data:HELLO\n");
            await _response.WriteAsync("\n");
            await _response.Body.FlushAsync();*/

            return _response.WriteSseEventAsync(serverSentEvent);

            /*await _response.WriteAsync("data: " + serverSentEvent.Data + "\n");
            await _response.WriteAsync("\n");
            await _response.Body.FlushAsync();*/

        }

    }
}
