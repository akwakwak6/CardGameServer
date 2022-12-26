using API.SSE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MainSseController : ControllerBase {

        private readonly ServerSentEventsService _SseService;

        public MainSseController(ServerSentEventsService sseService) {
            _SseService = sseService;
        }

        [HttpGet]
        public async Task Listen() {

            //await HttpContext.SSEInitAsync();//TODO
            HttpContext.Response.Headers.Add("Cache-Control", "no-cache");
            HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");
            await HttpContext.Response.Body.FlushAsync();

            ServerSentEventsClient client = new ServerSentEventsClient(HttpContext.Response);
            Guid clientId = _SseService.AddClient(client);

            Console.WriteLine("connect " + clientId);

            HttpContext.RequestAborted.WaitHandle.WaitOne();

            _SseService.RemoveClient(clientId);

            Console.WriteLine("disconnect " + clientId);

        }

    }
}
