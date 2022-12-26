using API.SSE;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]

    public class DemoSSEController : ControllerBase {

        private readonly ServerSentEventsService _serverSentEventsService;
        private readonly ServerSentEventsManager _sseTeamServ;

        public DemoSSEController(ServerSentEventsService serverSentEventsService, ServerSentEventsManager sseTeamServ) {
            _serverSentEventsService = serverSentEventsService;
            _sseTeamServ = sseTeamServ;
        }


        [HttpGet]
        public async Task Test() {

            //await HttpContext.SSEInitAsync();//TODO
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


        [HttpGet("teams")]
        public IActionResult GetTeams() {

            return Ok(_sseTeamServ.getAllTeams());

        }

        [HttpPost("postToTeam")]
        public IActionResult PostToTeam(Guid teamId) {

            _sseTeamServ.SendEventAsync(teamId, new Models.SseModel() { Data = new string[] { "Hello" } } );

            return Ok();

        }



        [HttpGet("createTeam")]
        public async Task CreateTeam() {
            //await HttpContext.SSEInitAsync();//TODO
            HttpContext.Response.Headers.Add("Cache-Control", "no-cache");
            HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");
            await HttpContext.Response.Body.FlushAsync();

            ServerSentEventsClient client = new ServerSentEventsClient(HttpContext.Response);

            Guid teamID = _sseTeamServ.CreateSseTeam();
            Guid clientId = _sseTeamServ.AddCientToTeam(teamID, client);

            Console.WriteLine("Create team " + teamID);

            HttpContext.RequestAborted.WaitHandle.WaitOne();

            _sseTeamServ.RemoveClientFromTeam(clientId, teamID);

        }

        [HttpGet("JoinTeam")]
        public async Task JoinTeam(Guid teamId) {

            //await HttpContext.SSEInitAsync();//TODO
            HttpContext.Response.Headers.Add("Cache-Control", "no-cache");
            HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");
            await HttpContext.Response.Body.FlushAsync();

            ServerSentEventsClient client = new ServerSentEventsClient(HttpContext.Response);

            Guid clientId = _sseTeamServ.AddCientToTeam(teamId, client);

            Console.WriteLine("join team " + clientId);

            HttpContext.RequestAborted.WaitHandle.WaitOne();

            _sseTeamServ.RemoveClientFromTeam(clientId, teamId);

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
