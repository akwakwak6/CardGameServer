using BLL.Models.PresiModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Presi {
    public class PresiTableManagerBL {

        private Dictionary<int,PresiTableBL> _Tables = new Dictionary<int,PresiTableBL>();

        public void CreateTable(int id) {
            _Tables.Add(id, new PresiTableBL());
        }

        public void AddPlayer(int tableId, PresiPlayerGameModel player, Action<PresiGameModel> cb) {
            GetTable(tableId).AddPlayer(player, cb);
        }

        public void RemovePlayer(int tableId,int playerID) {
            GetTable(tableId).RemovePlayer(playerID);
        }

        public void Ready(int tableId, int playerId) {
            GetTable(tableId).Ready(playerId);
        }

        public void SetCards(int tableId, int playerId, IEnumerable<int> cards) {
            GetTable(tableId).SetCards(playerId, cards);
        }


        private PresiTableBL GetTable(int tableId) {

            _Tables.TryGetValue(tableId, out PresiTableBL? t);
            if (t == null) {
                throw new Exception();
            }
            return t;
        }

    }
}
