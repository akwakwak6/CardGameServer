using BLL.Models.PresiModel;
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
            _Tables.TryGetValue(tableId, out PresiTableBL t);
            if (t == null) {
                throw new Exception();
            }
            t.AddPlayer(player, cb);
        }

    }
}
