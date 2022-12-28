using API.Controllers;
using API.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Runtime.Intrinsics.X86;

namespace API.SSE {
    public class ServerSentEventsManager {

        private readonly ConcurrentDictionary<int, ServerSentEventsService> _Teams = new ConcurrentDictionary<int, ServerSentEventsService>();

        public void CreateSseTeam(int teamId) {

            _Teams.TryAdd(teamId, new ServerSentEventsService());

        }

        public Guid AddCientToTeam(int teamId, ServerSentEventsClient client ) {
            if ( ! _Teams.TryGetValue(teamId, out ServerSentEventsService service))
                throw new NullReferenceException(nameof(service));//TODO better error manage
            return service.AddClient(client);
        }

        public int RemoveClientFromTeam(Guid client,int teamId) {
            if (!_Teams.TryGetValue(teamId, out ServerSentEventsService service))
                throw new NullReferenceException(nameof(service));//TODO better error manage
            return service.RemoveClient(client);
        }

        public void RemoveSseTeam(int teamId) {//TODO put private auto when no client connected
            _Teams.TryRemove(teamId, out ServerSentEventsService service);
            //TODO check if I have to remove all client ? close team when nobody in the team so no.
        }

        public ICollection<int> getAllTeams() {
            return _Teams.Keys;
        }

        public Task SendObjectAsync(int teamId, Object o) {

            SseModel sse = new SseModel() {//TODO same methode than sseService, so ...? move to SseModel ?
                Type = o.GetType().Name,
                Data = new string[] { JsonConvert.SerializeObject(o) }
            };

            return SendEventAsync(teamId, sse);
        }

        public Task SendEventAsync(int teamId,SseModel sse) {

            if (!_Teams.TryGetValue(teamId, out ServerSentEventsService service))
                throw new NullReferenceException(nameof(service));//TODO better error manage

            return service.SendEventAsync(sse);

        }

    }
}
