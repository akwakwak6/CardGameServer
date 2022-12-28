using BLL.Models.PresiModel;

namespace BLL.Services.Presi {
    internal class PresiTableBL {

        private List<Action<PresiGameModel>> _SendDataMth = new List<Action<PresiGameModel>>();

        private List<PresiPlayerGameModel> _Players = new List<PresiPlayerGameModel>();

        public void AddPlayer(PresiPlayerGameModel player, Action<PresiGameModel> cb) {

            Console.WriteLine("add player "+player);

            _SendDataMth.Add(cb);
            _Players.Add( player );

            UpdateDataTable();

        }

        private void UpdateDataTable() {
            PresiGameModel pgm = new PresiGameModel() { 
                Playing = 118,
                Players = _Players
            };

            foreach (Action<PresiGameModel> mth in _SendDataMth) {
                mth?.Invoke(pgm);
            }

        }

    }
}
