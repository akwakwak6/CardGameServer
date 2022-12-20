using Newtonsoft.Json;

namespace API.SSE {
    public static class ServerSentEventHelper {

        /*

        public static async Task SSEInitAsync(this HttpContext ctx) {
            ctx.Response.Headers.Add("Cache-Control", "no-cache");
            ctx.Response.Headers.Add("Content-Type", "text/event-stream");
            await ctx.Response.Body.FlushAsync();
        }

        public static async Task SSESendDataAsync(this HttpContext ctx, string data) {
            foreach (var line in data.Split('\n'))
                await ctx.Response.WriteAsync("data: " + line + "\n");

            await ctx.Response.WriteAsync("\n");
            await ctx.Response.Body.FlushAsync();
        }


        */
    }
}
