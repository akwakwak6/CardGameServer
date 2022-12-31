using BLL.Models.PresiModel;
using System.Diagnostics;

namespace BLL.Services.Presi {

    

    internal class PresiTableBL {

        enum GameState : ushort {
            INIT,
            WAITREADY,
            PLAY
        }

        private GameState _GS = GameState.INIT;
        private List<PlayerInGame> _Players = new List<PlayerInGame>();
        private List<int> _CenterCarte = new List<int>();

        public void AddPlayer(PresiPlayerGameModel player, Action<PresiGameModel> cb) {

            Console.WriteLine("add player "+player);
            _Players.Add( new PlayerInGame(player,cb) );

            if (_GS == GameState.INIT) {
                if (_Players.Count >= 2)
                    StartGame();
            }

            UpdateDataTable();

        }



        public void RemovePlayer(int playerID) {
            _Players.RemoveAll(p => p.Player.Id == playerID);
            UpdateDataTable();
        }

        public void Ready(int playerId) {

            _Players.ForEach(p => {
                if (p.Player.Id == playerId)
                    p.Player.IsPlaying = false;
            });

            if ( _Players.All(p => !p.Player.IsPlaying)) {
                Console.WriteLine("OK deals cards");
                DealCards();
            }  

            UpdateDataTable();
            Console.WriteLine( "Ready "+playerId );
        }

        public void SetCards(int playerId, IEnumerable<int> cards) {
            Console.WriteLine("set cards "+playerId+" " + cards.Count()+ " first "+cards.First());

            if ( !_Players.Where(p => p.Player.Id == playerId).First().Player.IsPlaying ) return;

            int? i = null;
            int cpt = 0;

            _Players.ForEach( p => {  
            
                if(p.Player.Id  == playerId) {
                    
                    p.Player.IsPlaying = false;
                    i = cpt;
                    p.Cards.RemoveAll(c => cards.Contains(c));
                    //p.Cards.Remove(cards);
                }
                cpt++;
                
            });

            _CenterCarte = cards.ToList();

            if (i is not null) {
                _Players[(int)(++i) % _Players.Count].Player.IsPlaying = true;
            }

            UpdateDataTable();


        }


        private void UpdateDataTable() {

            PresiGameModel pgm = new PresiGameModel() { 
                CenterCarte = _CenterCarte,
                //ShowReady = false
            };
            _Players.ForEach(p => {
                p.Player.NbCard = p.Cards.Count;
            });

            if (_GS == GameState.INIT) {
                foreach (PlayerInGame pig in _Players) {
                    pgm.Players = _Players.Where(p => p.Player.Id != pig.Player.Id).Select(p => p.Player).ToList();
                    pgm.Me = _Players.Single(p => p.Player.Id == pig.Player.Id).Player; 
                    pig.CallBack(pgm);
                }
                return;
            }

            if (_GS == GameState.WAITREADY) {
                foreach (PlayerInGame pig in _Players) {
                    pgm.Players = _Players.Where(p => p.Player.Id != pig.Player.Id).Select(p => p.Player).ToList();
                    pgm.Me = _Players.Single(p => p.Player.Id == pig.Player.Id).Player;
                    pgm.ShowReady = pig.Player.IsPlaying;
                    pig.CallBack(pgm);
                }
                return;
            }

            if( _GS == GameState.PLAY) {
                foreach (PlayerInGame pig in _Players) {
                    pgm.Players = _Players.Where(p => p.Player.Id != pig.Player.Id).Select(p => p.Player).ToList();
                    pgm.Me = _Players.Single(p => p.Player.Id == pig.Player.Id).Player;
                    pgm.ShowReady = pig.Player.IsPlaying;
                    pgm.MyHand = pig.Cards;

                    pig.CallBack(pgm);

                }
                return;
            }

            

        }

        private void StartGame() {
            _GS = GameState.WAITREADY;
            _CenterCarte = new List<int>();
            _Players.ForEach(p => {
                p.Player.IsPlaying = true;
                p.Cards.Clear();
                p.Player.NbCard = 0;
            });
        }

        private void DealCards() {
            _GS = GameState.PLAY;
            List<int> cards = Enumerable.Range(0, 54).ToList();
            Random rand = new Random();
            cards = cards.OrderBy( _ => rand.Next()).ToList();

            foreach (var o in cards.Select((c, i) => new { c, i })) {

                _Players[ o.i % _Players.Count ].Cards.Add(o.c);

            }

            _Players.ForEach(p => {
                p.Cards.Sort( (c1,c2) => CompareCard(c1,c2) );
            });

            _Players.ForEach(p => { if (p.Cards.Contains(1)) p.Player.IsPlaying = true; });

        }

        private int CompareCard(int a,int b) {
            int a2 = a % 13;
            int b2 = b % 13;
            return a2 - b2;
        }

        private class PlayerInGame {

            public Action<PresiGameModel> CallBack { get; }
            public PresiPlayerGameModel Player { get; }

            public List<int> Cards = new List<int>();

            public PlayerInGame(PresiPlayerGameModel p, Action<PresiGameModel> callBack) {
                Player = p;
                CallBack = callBack;
            }
        }

    }
}
