using API.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Runtime.Intrinsics.X86;

namespace API.SSE {
    public class ServerSentEventsManager {

        private readonly ConcurrentDictionary<Guid, ServerSentEventsService> _Teams = new ConcurrentDictionary<Guid, ServerSentEventsService>();

        public Guid CreateSseTeam() {

            Guid teamId = Guid.NewGuid();
            _Teams.TryAdd(teamId, new ServerSentEventsService());
            return teamId;

        }

        public Guid AddCientToTeam(Guid teamId, ServerSentEventsClient client) {
            Console.WriteLine(teamId);
            Console.WriteLine(_Teams.Count);
            if ( ! _Teams.TryGetValue(teamId, out ServerSentEventsService service))
                throw new NullReferenceException(nameof(service));//TODO better error manage
            return service.AddClient(client);
        }

        public void RemoveClientFromTeam(Guid client,Guid teamId) {
            if (!_Teams.TryGetValue(teamId, out ServerSentEventsService service))
                throw new NullReferenceException(nameof(service));//TODO better error manage
            service.RemoveClient(client);
        }

        public void RemoveSseTeam(Guid teamId) {//TODO put private auto when no client connected
            _Teams.TryRemove(teamId, out ServerSentEventsService service);
            //TODO check if I have to remove all client ? close team when nobody in the team so no.
        }

        public Task SendObjectAsync(Guid teamId, Object o) {

            SseModel sse = new SseModel() {//TODO same methode than sseService, so ...? move to SseModel ?
                Type = o.GetType().Name,
                Data = new string[] { JsonConvert.SerializeObject(o) }
            };

            return SendEventAsync(teamId,sse);
        }

        public Task SendEventAsync(Guid teamId,SseModel sse) {

            if (!_Teams.TryGetValue(teamId, out ServerSentEventsService service))
                throw new NullReferenceException(nameof(service));//TODO better error manage

            return service.SendEventAsync(sse);

        }

    }
}
