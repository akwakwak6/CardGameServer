using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.SSE {
    internal static class ServerSentEventHelper {

        internal static async Task<ServerSentEventsClient> InitAndGetSseClient(this ControllerBase ctrl) {
            ctrl.HttpContext.Response.Headers.Add("Cache-Control", "no-cache");
            ctrl.HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");
            await ctrl.HttpContext.Response.Body.FlushAsync();
            return new ServerSentEventsClient(ctrl.HttpContext.Response);
        }

        internal static async Task WriteSseEventAsync(this HttpResponse response, SseModel sse) {
            if (!String.IsNullOrWhiteSpace(sse.Id))
                await response.WriteSseEventFieldAsync("id", sse.Id);

            if (!String.IsNullOrWhiteSpace(sse.Type))
                await response.WriteSseEventFieldAsync("event", sse.Type);

            if (sse.Data != null) {
                foreach (string data in sse.Data)
                    await response.WriteSseEventFieldAsync("data", data);
            }

            await response.WriteAsync("\n");
            await response.Body.FlushAsync();
        }

        private static Task WriteSseEventFieldAsync(this HttpResponse response, string field, string data) {
            return response.WriteAsync($"{field}: {data}\n");
        }

    }
}
