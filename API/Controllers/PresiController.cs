using API.Infrastructure;
using API.SSE;
using BLL.Models;
using BLL.Models.PresiModel;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PresiController : ControllerBase {

        private PresiService _PresiSrv;
        private readonly ServerSentEventsService _SseService;
        private readonly ServerSentEventsManager _sseTeamServ;
        private readonly TokenManager _TokonSrv;

        private ServerSentEventsClient client;

        public PresiController (PresiService presiSrv, ServerSentEventsService sseService, ServerSentEventsManager sseTeamServ, TokenManager tknSrv) {
            _PresiSrv = presiSrv;
            _SseService = sseService;
            _sseTeamServ = sseTeamServ;
            _TokonSrv = tknSrv;
        }

        [Authorize]
        [HttpPost("createTable")]
        public IActionResult CreateTable() {

            /*HttpContext.Response.Headers.Add("Cache-Control", "no-cache");
            HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");
            await HttpContext.Response.Body.FlushAsync();*/

            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null) return BadRequest();

            try {
                string psd = identity.FindFirst("Pseudo").Value;
                int USerId = int.Parse(identity.FindFirst("UserId").Value);
                _PresiSrv.CreateTable(USerId, psd, out int tableId);
                //_sseTeamServ.CreateSseTeam(tableId);
                _sseTeamServ.CreateSseTeam(tableId);
                //HttpContext.RequestAborted.WaitHandle.WaitOne();

                //_PresiSrv.LeftTable(tableId, playerId);
                return Ok();

            } catch (Exception ex) {//TODO redo catch
                return BadRequest(ex);
            }
        }

        [HttpGet("joinTable")]
        public async Task JoinTable(int tableId,string token) {

            int? userId = null;

            if (token != "null") {
                userId = int.Parse(_TokonSrv.ReadToken(token, "UserId"));
            }


            HttpContext.Response.Headers.Add("Cache-Control", "no-cache");
            HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");
            await HttpContext.Response.Body.FlushAsync();

            client = new ServerSentEventsClient(HttpContext.Response);
            //Guid playerGuid = _sseTeamServ.AddCientToTeam(tableId, client);//TODO create teams if no teams 

            int playerId = _PresiSrv.JoinTable(tableId,SendDataTable,userId);

            //client.SendEventAsync(new Models.SseModel() { Type = "playerID", Data = new List<string>() { "{\"playerId\":" + playerId + " } " } } );

            Console.WriteLine("joined table "+ tableId + " player id :" + playerId);
            HttpContext.RequestAborted.WaitHandle.WaitOne();

            _PresiSrv.LeftTable(tableId, playerId);
            //await LeaveTable(tableId, playerGuid, playerId);

        }

        [HttpGet("ready")]
        public IActionResult SetReady(int tableId,int playerId) {
            //TODO useToken
            _PresiSrv.SetReady(tableId, playerId);
            return Ok();
        }

        [HttpPost("setCards")]
        public IActionResult setCards(int tableId, int playerId, List<int> cards) {
            //TODO useToken
            _PresiSrv.SetCards( tableId, playerId, cards);
            return Ok();
        }

        private void SendDataTable(PresiGameModel data) {

            client.SendEventAsync(_SseService.getSSE(data));

        }

    }
}
