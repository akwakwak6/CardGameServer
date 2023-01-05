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

        private readonly PresiService _PresiService;
        private readonly EventService _EventSrv;

        private ServerSentEventsClient client;

        public MainSseController(PresiService presiService, EventService eventSrv) {
            _PresiService = presiService;
            _EventSrv = eventSrv;
        }

        [HttpGet]
        public async Task Listen() {

            client = await this.InitAndGetSseClient();

            TableListener(_PresiService.getTableList());
            _EventSrv.AddTableListener(TableListener);

            HttpContext.RequestAborted.WaitHandle.WaitOne();

            _EventSrv.RemoveTableListener(TableListener);

        }

        private void TableListener(PresiTableList tables) {
            client.SendObjectAsync(tables);
        }

    }
}
