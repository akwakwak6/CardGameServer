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
        private readonly TokenManager _TokonSrv;

        private ServerSentEventsClient client;

        public PresiController (PresiService presiSrv, TokenManager tknSrv) {
            _PresiSrv = presiSrv;
            _TokonSrv = tknSrv;
        }

        [Authorize]
        [HttpPost("createTable")]
        public IActionResult CreateTable() {

            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null) return BadRequest();

            try {
                string psd = identity.FindFirst("Pseudo").Value;
                int USerId = int.Parse(identity.FindFirst("UserId").Value);
                _PresiSrv.CreateTable(USerId, psd, out int tableId);
                return Ok();

            } catch (Exception ex) {//TODO redo catch
                return BadRequest();
            }
        }

        [HttpGet("joinTable")]
        public async Task JoinTable(int tableId,string token) {

            int? userId = null;

            if (token != "null") {
                userId = int.Parse(_TokonSrv.ReadToken(token, "UserId"));
            }

            client = await this.InitAndGetSseClient();

            int playerId = _PresiSrv.JoinTable(tableId,SendDataTable,userId);

            HttpContext.RequestAborted.WaitHandle.WaitOne();

            _PresiSrv.LeftTable(tableId, playerId);

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
            client.SendObjectAsync(data);
        }

    }
}
