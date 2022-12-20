using API.SSE;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]


    public class DemoSSEController : ControllerBase {

        private readonly ServerSentEventsService _serverSentEventsService;

        public DemoSSEController(ServerSentEventsService serverSentEventsService) {
            _serverSentEventsService = serverSentEventsService;
        }


        [HttpGet]
        public async Task Test() {

            //await HttpContext.SSEInitAsync();
            HttpContext.Response.Headers.Add("Cache-Control", "no-cache");
            HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");
            await HttpContext.Response.Body.FlushAsync();

            ServerSentEventsClient client = new ServerSentEventsClient(HttpContext.Response);
            Guid clientId = _serverSentEventsService.AddClient(client);

            Console.WriteLine("connect "+clientId);

            HttpContext.RequestAborted.WaitHandle.WaitOne();

            _serverSentEventsService.RemoveClient(clientId);

            Console.WriteLine("disconnect " + clientId);


        }

        public class Tp {
            public int Id { get; set; }
            public string Name { get; set; }

            public List<string> List { get; set; }
        }

            [HttpPost]
        public IActionResult Send(string msg) {

            Tp tp = new Tp() { Id = 2, Name = "Toto" };

            tp.List = new List<string>() { "ichi" , "ni" , "san" } ;

            _serverSentEventsService.SendObjectAsync(tp);

                

            return Ok();
        }

    }
}
