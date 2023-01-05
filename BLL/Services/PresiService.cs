using Microsoft.EntityFrameworkCore;
using BLL.Models.PresiModel;
using BLL.Services.Presi;
using Entities.Presi;
using DAL;

namespace BLL.Services {
    public class PresiService {

        private CardGameDbContext _DB;
        private PresiTableManagerBL _TableMgnSrv;
        private EventService _EventSrv;

        public PresiService(CardGameDbContext db, EventService eventSrv, PresiTableManagerBL tableMgn) {
            _DB = db;
            _EventSrv = eventSrv;
            _TableMgnSrv = tableMgn;
        }

        public void CreateTable(int userId,string pseudo,out int tableId) {

            PresiTable table = new PresiTable() {
                UserId = userId,
                Players = new List<PresiPlayer>()
            };

            _DB.Add(table);

            _DB.SaveChanges();

            SendTableList();
            tableId = table.Id;

            _TableMgnSrv.CreateTable(tableId);

        }

        private void SendTableList() {
                _EventSrv.tableEvent?.Invoke(getTableList());
        }

        public PresiTableList getTableList() {

            using (CardGameDbContext cxt = new CardGameDbContext()) {
                return new PresiTableList() { Tables = cxt.PresiTables.Include(t => t.Players.Where(p => p.IsPlaying)).Where(t => t.IsActive).ToList() };
            }
                
        }

        public int JoinTable(int tableId,Action<PresiGameModel> cb,int? userId = null) {

            PresiPlayer plDB = new PresiPlayer() { PresiTableId = tableId };
            PresiPlayerGameModel plBL = new PresiPlayerGameModel() { };

            _DB.Add(plDB);

            _DB.SaveChanges();

            if (userId is null) {
                plBL.Pseudo = "Visitor:" + plDB.Id;
            } else {
                plBL.Pseudo = _DB.Users.Find(userId).Pseudo;//TODO if bad userId do ... ?
            }
            plBL.Id = plDB.Id;

            _TableMgnSrv.AddPlayer(tableId, plBL, cb);

            SendTableList();

            return plDB.Id;
        }

        public void LeaveTable(int tableId,int playerId) {//TODO no need table ID

            PresiTable? t = _DB.PresiTables.Include(t => t.Players.Where(p => p.IsPlaying)).Where(t => t.Id == tableId).FirstOrDefault();
            if (t is null) return;

            PresiPlayer? p = t.Players.Where(t => t.Id == playerId).FirstOrDefault();
            if (p is null) return;
            p.IsPlaying = false;

            _TableMgnSrv.RemovePlayer(tableId, playerId);

            if (t.Players.Count == 1)
                t.IsActive = false;

            _DB.SaveChanges();
            SendTableList();

        }

        public void SetReady(int tableId, int playerId) {
            _TableMgnSrv.Ready(tableId, playerId);
        }

        public void SetCards(int tableId, int playerId, List<int> cards) {
            _TableMgnSrv.SetCards(tableId, playerId, cards);
        }
    }
}
