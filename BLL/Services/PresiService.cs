using BLL.Models.PresiModel;
using BLL.Services.Presi;
using DAL;
using Entities.Presi;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BLL.Services {
    public class PresiService {

        private CardGameDbContext _DB;

        private EventService _EventSrv;

        private PresiTableManagerBL _TableMgnSrv;

        public PresiService(CardGameDbContext db, EventService eventSrv, PresiTableManagerBL tableMgn) {

            _DB = db;
            _EventSrv = eventSrv;
            _TableMgnSrv = tableMgn;
        }

        public void CreateTable(int userId,string pseudo,out int tableId) {

            Entities.Presi.PresiTable table = new Entities.Presi.PresiTable() {
                UserId = userId,
                Players = new List<PresiPlayer>()
            };
            //PresiPlayer p = new PresiPlayer() { Pseudo = pseudo };
            //table.Players.Add(p);

            _DB.Add(table);

            _DB.SaveChanges();

            SendTableList();
            tableId = table.Id;

            _TableMgnSrv.CreateTable(tableId);
            //_TableMgnSrv.AddPlayer(tableId,new PresiPlayerB());

            Console.WriteLine("create table " + tableId + " userId id :"+ userId);

        }

        private void SendTableList() {
            _EventSrv.tableEvent?.Invoke(getTableList());
        }

        public PresiTableList getTableList() {
            //TODO understand why, when put player IsPlaying false, get this player in the playerList the first time of the call ? ? ? 

            List<Entities.Presi.PresiTable> tables = _DB.PresiTables.Include(t => t.Players).Where(t => t.IsActive).ToList();
            List<Entities.Presi.PresiTable> tables2 = new List<Entities.Presi.PresiTable>();

            foreach (var table in tables) {//TODO remove this foreach
                List<PresiPlayer> pls = table.Players.Where( p => p.IsPlaying ).ToList();
                Entities.Presi.PresiTable tp = table;
                tp.Players = pls;
                tables2.Add(tp);
            }

            return new PresiTableList() {//TODO send a array. not a model with just a list
                Tables = tables2
            };

            //return new PresiTableList() { Tables = _DB.PresiTables.Include(t => t.Players.Where(p => p.IsPlaying)).Where(t => t.IsActive).ToList() };
        }

        public int JoinTable(int tableId,Action<PresiGameModel> cb,int? userId = null) {

            PresiPlayer plDB = new PresiPlayer() { PresiTableId = tableId };
            PresiPlayerGameModel plBL = new PresiPlayerGameModel() { };

            //PresiTable table = _DB.PresiTables.Include(t => t.Players).Where(t => t.Id == tableId).First();
            //just add player no need to take table then add
            //table.Players.Add( p );

            _DB.Add(plDB);

            _DB.SaveChanges();

            if (userId is null) {
                plBL.Pseudo = "Visito:" + plDB.Id;
            } else {
                plBL.Pseudo = _DB.Users.Find(userId).Pseudo;//TODO if bad userId do ... ?
            }
            plBL.Id = plDB.Id;

            _TableMgnSrv.AddPlayer(tableId, plBL, cb);

            SendTableList();

            return plDB.Id;
        }

        public void LeftTable(int tableId,int playerId) {//TODO no need table ID / left to leave


            _TableMgnSrv.RemovePlayer(tableId, playerId);
            Console.WriteLine("left table " + tableId + " player id :" + playerId);
            PresiPlayer? player = _DB.PresiPlayers.Find(playerId);

            if (player is null) return;
            player.IsPlaying = false;


            PresiTable? table = _DB.PresiTables.Find(tableId);

            if (table is null) return;

            if (table.Players.Count == 1) {
                Console.WriteLine("no player on table "+ tableId);
                table.IsActive = false;
            }
            _DB.SaveChanges();
            SendTableList();

        }

        public void SetReady(int tableId, int playerId) {
            _TableMgnSrv.Ready(tableId, playerId);
        }

        public void SetCards(int tableId, int playerId, IEnumerable<int> cards) {
            _TableMgnSrv.SetCards(tableId, playerId, cards);
        }
    }
}
