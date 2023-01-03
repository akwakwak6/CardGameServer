using BLL.Models.PresiModel;
using System.Diagnostics;
using System.Numerics;

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
        private int _Position = 0;

        public void AddPlayer(PresiPlayerGameModel player, Action<PresiGameModel> cb) {

            Console.WriteLine("add player "+player);
            _Players.Add( new PlayerInGame(player,cb) );

            if (_GS == GameState.INIT) {
                if (_Players.Count >= 3)
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

        public void SetCards(int playerId, List<int> cards) {

            //Check if player can play
            if (!_Players.Where(p => p.Player.Id == playerId).First().Player.IsPlaying) return;

            //find player index
            int index = _Players.FindIndex(p => p.Player.Id == playerId);

            //check has cards
            if (!cards.All(c => _Players[index].Cards.Contains(c))) return;

            //TODO check can play cards ?
            
            if(cards.Count == 0) {//if player pass
                _Players[index].Player.Passed = true;
                FindNextPlayer(index);
            } else {
                _CenterCarte = cards;
                _Players[index].Cards.RemoveAll(c => cards.Contains(c));
                if(_Players[index].Cards.Count == 0) {//player finish
                    PlayerFinish(index);
                } else {
                    if (cards.First() % 13 != 0) {//cards played is not 2
                        FindNextPlayer(index);
                    } 
                }
            }

            UpdateDataTable();


        }

        private bool IsFinish() {
            return _Players.Where(p => p.Cards.Count != 0).Count() == 1;
        }

        private void FindNextPlayer(int index) {
            _Players[index].Player.IsPlaying = false;

            for(int i = 1; i < _Players.Count; i++) {
                if( !_Players[ (index + i) % _Players.Count ].Player.Passed && _Players[(index + i) % _Players.Count].Cards.Count != 0) {
                    _Players[(index + i) % _Players.Count].Player.IsPlaying = true;
                    if (_Players.Where(p => !p.Player.Passed && p.Position is null).Count()  == 1) {
                        //if he is the only one who didn't pass, clean center
                        _CenterCarte = new List<int>();
                        _Players.ForEach(p => p.Player.Passed = false);
                    }
                    return;
                }
            }
        }

        private void PlayerFinish(int index) {
            Console.WriteLine($" player {_Players[index].Player.Id} finish");
            _Players[index].Position = _Position;
            _Position++;
            if (IsFinish()) {
                Console.WriteLine("game finish");
                GiveRoleFctPosition();
            } else {
                FindNextPlayer(index);
            }
            //TODO
            //he was last ?
            // set position .... go on
        }

        private void GiveRoleFctPosition() {

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
                    pgm.MyHand = new List<int>();

                    pig.CallBack(pgm);
                }
                return;
            }

            if (_GS == GameState.WAITREADY) {
                foreach (PlayerInGame pig in _Players) {
                    pgm.Players = _Players.Where(p => p.Player.Id != pig.Player.Id).Select(p => p.Player).ToList();
                    pgm.Me = _Players.Single(p => p.Player.Id == pig.Player.Id).Player;
                    pgm.ShowReady = pig.Player.IsPlaying;
                    pgm.MyHand = new List<int>();

                    pig.CallBack(pgm);
                }
                return;
            }

            if( _GS == GameState.PLAY) {
                foreach (PlayerInGame pig in _Players) {
                    pgm.Players = _Players.Where(p => p.Player.Id != pig.Player.Id).Select(p => p.Player).ToList();
                    pgm.Me = _Players.Single(p => p.Player.Id == pig.Player.Id).Player;
                    pgm.ShowReady = false;
                    pgm.MyHand = pig.Cards;

                    pig.CallBack(pgm);

                }
                return;
            }

            

        }

        private void StartGame() {
            _GS = GameState.WAITREADY;
            _Position = 0;
            _CenterCarte = new List<int>();
            _Players.ForEach(p => {
                p.Player.IsPlaying = true;
                p.Cards.Clear();
                p.Player.NbCard = 0;
                p.Player.Passed = false;
                p.Position = null;
            });
        }

        private void DealCards() {
            _GS = GameState.PLAY;
            List<int> cards = Enumerable.Range(0, 5).ToList();//TODO change by 52
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

            public int? Position = null;

            public PlayerInGame(PresiPlayerGameModel p, Action<PresiGameModel> callBack) {
                Player = p;
                CallBack = callBack;
            }
        }

    }
}
