using API.SSE;
using BLL.Models.PresiModel;
using BLL.Services;
using Entities.Presi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class MainSseController : ControllerBase {

        private readonly ServerSentEventsService _SseService;
        private readonly EventService _EventService;
        private readonly PresiService _PresiService;

        private ServerSentEventsClient client;

        public MainSseController(ServerSentEventsService sseService , EventService eventSrv, PresiService presiService) {
            _SseService = sseService;
            _EventService = eventSrv;
            _PresiService = presiService;
        }

        [HttpGet]
        public async Task Listen() {

            //await HttpContext.SSEInitAsync();//TODO
            HttpContext.Response.Headers.Add("Cache-Control", "no-cache");
            HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");
            await HttpContext.Response.Body.FlushAsync();

            client = new ServerSentEventsClient(HttpContext.Response);

            //Guid clientId = _SseService.AddClient(client);

            //Console.WriteLine("connect " + clientId);

            TableListener(_PresiService.getTableList());
            _EventService.AddTableListener(TableListener);


            HttpContext.RequestAborted.WaitHandle.WaitOne();


            _EventService.RemoveTableListener(TableListener);

            //_SseService.RemoveClient(clientId);

            //Console.WriteLine("disconnect " + clientId);

        }

        private void TableListener(PresiTableList tables) {
            //_SseService.SendObjectAsync(tables);
            client.SendEventAsync(_SseService.getSSE(tables));
        }

    }
}
