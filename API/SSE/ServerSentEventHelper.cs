using API.Models;

namespace API.SSE {
    internal static class ServerSentEventHelper {

        internal static async Task WriteSseEventAsync(this HttpResponse response, SseModel sse) {
            if (!String.IsNullOrWhiteSpace(sse.Id))
                await response.WriteSseEventFieldAsync("id", sse.Id);

            if (!String.IsNullOrWhiteSpace(sse.Type))
                await response.WriteSseEventFieldAsync("event", sse.Type);

            if (sse.Data != null) {
                foreach (string data in sse.Data)
                    await response.WriteSseEventFieldAsync("data", data);
            }

            await response.WriteSseEventBoundaryAsync();
            await response.Body.FlushAsync();
        }

        private static Task WriteSseEventFieldAsync(this HttpResponse response, string field, string data) {
            return response.WriteAsync($"{field}: {data}\n");
        }

        private static Task WriteSseEventBoundaryAsync(this HttpResponse response) {
            return response.WriteAsync("\n");
        }
    }
}
