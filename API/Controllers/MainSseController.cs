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

        private readonly EventService _EventService;
        private readonly PresiService _PresiService;

        private ServerSentEventsClient client;

        public MainSseController(EventService eventSrv, PresiService presiService) {
            _EventService = eventSrv;
            _PresiService = presiService;
        }

        [HttpGet]
        public async Task Listen() {

            client = await this.InitAndGetSseClient();

            TableListener(_PresiService.getTableList());
            _EventService.AddTableListener(TableListener);

            HttpContext.RequestAborted.WaitHandle.WaitOne();

            _EventService.RemoveTableListener(TableListener);

        }

        private void TableListener(PresiTableList tables) {
            client.SendObjectAsync(tables);
        }

    }
}
