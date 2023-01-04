using BLL.Models.PresiModel;
using System.Collections;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Threading;

namespace BLL.Services.Presi {

    

    internal class PresiTableBL {

        enum GameState : ushort {
            INIT,
            WAITREADY,
            EXCHANGE,
            SHOWNEWCARD,
            PLAY
        }

        private const int NB_PLAYER_MIN = 3;

        private GameState _GS = GameState.INIT;
        private List<PlayerInGame> _Players = new List<PlayerInGame>();
        private List<int> _CenterCarte = new List<int>();
        private int _Position = 0;

        public void AddPlayer(PresiPlayerGameModel player, Action<PresiGameModel> cb) {

            Console.WriteLine("add player "+player);
            _Players.Add( new PlayerInGame(player,cb) );

            if (_GS == GameState.INIT) {
                if (_Players.Count >= NB_PLAYER_MIN)
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

        private void ExchangeCard() {
            int presiI = _Players.FindIndex(p => p.Player.Role == PresiRoles.President);
            int bumI = _Players.FindIndex(p => p.Player.Role == PresiRoles.Bum);
            //TODO redo this code cleaner, too redundant
            if (presiI != -1 && bumI != -1) {
                _Players[presiI].ExhangeSelected.ForEach(c => _Players[bumI].Cards.Add(c));
                _Players[bumI].ExhangeSelected.ForEach(c => _Players[presiI].Cards.Add(c));
                _Players[presiI].Cards.Sort((a, b) => a % 13 - b % 13);
                _Players[bumI].Cards.Sort((a, b) => a % 13 - b % 13);
                _Players[presiI].NewCards = _Players[bumI].ExhangeSelected;
                _Players[bumI].NewCards = _Players[presiI].ExhangeSelected;
            }

            int vicePresiI = _Players.FindIndex(p => p.Player.Role == PresiRoles.VicePresident);
            int viceBumI = _Players.FindIndex(p => p.Player.Role == PresiRoles.ViceBum);

            if (vicePresiI != -1 && viceBumI != -1) {
                _Players[vicePresiI].ExhangeSelected.ForEach(c => _Players[viceBumI].Cards.Add(c));
                _Players[viceBumI].ExhangeSelected.ForEach(c => _Players[vicePresiI].Cards.Add(c));
                _Players[vicePresiI].Cards.Sort((a, b) => a % 13 - b % 13);
                _Players[viceBumI].Cards.Sort((a, b) => a % 13 - b % 13);
                _Players[vicePresiI].NewCards = _Players[viceBumI].ExhangeSelected;
                _Players[viceBumI].NewCards = _Players[vicePresiI].ExhangeSelected;
            }

        }

        public void SetCards(int playerId, List<int> cards) {

            //Check if player can play
            if (!_Players.Where(p => p.Player.Id == playerId).First().Player.IsPlaying) return;

            //find player index
            int index = _Players.FindIndex(p => p.Player.Id == playerId);

            //check has cards
            if (!cards.All(c => _Players[index].Cards.Contains(c))) return;

            //TODO check can play cards ?

            if (_GS == GameState.PLAY) {
                if (cards.Count == 0) {//if player pass
                    _Players[index].Player.Passed = true;
                    FindNextPlayer(index);
                } else {
                    _CenterCarte = cards;
                    _Players[index].Cards.RemoveAll(c => cards.Contains(c));
                    if (_Players[index].Cards.Count == 0) {//player finish
                        PlayerFinish(index);
                    } else {
                        if (cards.First() % 13 != 0) {//cards played is not 2
                            FindNextPlayer(index);
                        } else {
                            //TODO keep or remove ?
                            _Players.ForEach(p => p.Player.Passed = false);
                        }
                    }
                }
            }

            if (_GS == GameState.EXCHANGE) {
                _Players[index].Cards.Remove(cards.First());
                _Players[index].Exhange.Remove(cards.First());
                _Players[index].ExhangeSelected.Add(cards.First());
                _Players[index].cardToChange--;
                if (_Players[index].cardToChange == 0)
                    _Players[index].Player.IsPlaying = false;

                if (_Players.All(p => !p.Player.IsPlaying)) {
                    _GS = GameState.SHOWNEWCARD;
                    ExchangeCard();
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
                _Players.FindLast(p => p.Cards.Count != 0).Position = _Position;
                //TODO may be he left
                GiveRoleFctPosition();
                waitAndStartNewGame();
            } else {
                FindNextPlayer(index);
            }
        }

        private int GetPresiCardVal(int c) {
            int cTp = c % 13;
            if (cTp == 0) cTp = 13;
            return cTp;
        }

        private List<int> SortCardGoodToBad(List<int> cards) {
            return cards.OrderByDescending(c => GetPresiCardVal(c)).ToList();
        }

        private List<int> SelectBestCards(List<int> cards,int nbMin ) {
            //TODO if cards val is in best, take all cards with same val
            return SortCardGoodToBad(cards).Take(nbMin).ToList();
        }

        private List<int> SelectWorstCards(List<int> cards, int nb) {
            return SortCardGoodToBad(cards).Skip(cards.Count - nb).ToList();
        }

        private async Task waitAndStartNewGame() {
            await Task.Delay(3000);
            if (_Players.Count >= NB_PLAYER_MIN)
                StartGame();
            else
                _GS = GameState.INIT;
            UpdateDataTable();
        }

        private void GiveRoleFctPosition() {

            //manage player joined and left => create a sortedList
            int cpt = 0;
            SortedList<int,int> positionIndex = new SortedList<int, int>();
            _Players.ForEach(p => {
                p.Player.Role = PresiRoles.Neutre;
                positionIndex.Add(p.Position ?? cpt + _Players.Count , cpt);
                cpt++;
            });

            //list with index soreted by position, if position = null => last added = last
            List<int> positions = positionIndex.Values.ToList();

            if (_Players.Count == 2) {
                _Players[positions[0]].Player.Role = PresiRoles.President;
                _Players[positions[1]].Player.Role = PresiRoles.Bum;
            }else if ( _Players.Count == 3) {
                _Players[positions[0]].Player.Role = PresiRoles.President;
                _Players[positions[2]].Player.Role = PresiRoles.Bum;
            }else if (_Players.Count > 3) {
                _Players[positions[0]].Player.Role = PresiRoles.President;
                _Players[positions[1]].Player.Role = PresiRoles.VicePresident;
                _Players[positions[positions.Count - 2]].Player.Role = PresiRoles.ViceBum;
                _Players[positions.Last()].Player.Role = PresiRoles.Bum;
            }

        }


        private void UpdateDataTable() {

            PresiGameModel pgm = new PresiGameModel() { 
                CenterCarte = _CenterCarte,
                ChangeCards = new List<int>(),
                NewCards = new List<int>()
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

            if (_GS == GameState.EXCHANGE) {
                foreach (PlayerInGame pig in _Players) {
                    pgm.Players = _Players.Where(p => p.Player.Id != pig.Player.Id).Select(p => p.Player).ToList();
                    pgm.Me = _Players.Single(p => p.Player.Id == pig.Player.Id).Player;
                    pgm.ShowReady = false;
                    pgm.MyHand = pig.Cards;
                    pgm.ChangeCards = pig.Exhange;
                    pig.CallBack(pgm);
                }
                return;
            }

            if (_GS == GameState.SHOWNEWCARD) {
                SetFirtPlayer();
                foreach (PlayerInGame pig in _Players) {
                    pgm.NewCards = pig.NewCards;
                    pgm.Players = _Players.Where(p => p.Player.Id != pig.Player.Id).Select(p => p.Player).ToList();
                    pgm.Me = _Players.Single(p => p.Player.Id == pig.Player.Id).Player;
                    pgm.ShowReady = false;
                    pgm.MyHand = pig.Cards;
                    pig.CallBack(pgm);
                }
                _GS = GameState.PLAY;
                return;
            }

            if ( _GS == GameState.PLAY) {
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
                //p.Position = null;
                p.Exhange.Clear();
                p.ExhangeSelected.Clear();
                p.cardToChange = 0;
            });
        }

        private void DealCards() {
            
            List<int> cards = Enumerable.Range(0, 52).ToList();//TODO put 52 | if more than X players add decks
            Random rand = new Random();
            cards = cards.OrderBy( _ => rand.Next()).ToList();

            foreach (var o in cards.Select((c, i) => new { c, i })) {

                _Players[ o.i % _Players.Count ].Cards.Add(o.c);

            }

            _Players.ForEach(p => {
                p.Cards.Sort( (a,b) => a % 13 - b % 13);
            });


            //if all players are neutre => start directely else exchange cards fct role
            if(_Players.All(p => p.Player.Role == PresiRoles.Neutre)) {
                _GS = GameState.PLAY;
                SetFirtPlayer();
            } else {
                _GS = GameState.EXCHANGE;
                GiveRoleFctPosition();
                _Players.ForEach(p => p.Position = null);
                SetPlayingFctRole();
            }
            
        }

        private void SetFirtPlayer() {
            _Players.ForEach(p => {
                p.Player.IsPlaying = p.Cards.Contains(0);
            });
        }

        private void SetPlayingFctRole() {
            _Players.ForEach(p => {
                p.Player.IsPlaying = p.Player.Role != PresiRoles.Neutre;

                switch (p.Player.Role) {
                    case PresiRoles.President:
                        p.Exhange = SelectWorstCards(p.Cards, p.Cards.Count - 2);
                        p.cardToChange = 2;
                        break;
                    case PresiRoles.VicePresident:
                        p.Exhange = SelectWorstCards(p.Cards, p.Cards.Count - 2);
                        p.cardToChange = 1;
                        break;
                    case PresiRoles.ViceBum:
                        p.Exhange = SelectBestCards(p.Cards, 1);
                        p.cardToChange = 1;
                        break;
                    case PresiRoles.Bum:
                        p.Exhange = SelectBestCards(p.Cards, 2);
                        p.cardToChange = 2;
                        break;
                }


            });
        }

        private class PlayerInGame {

            public Action<PresiGameModel> CallBack { get; }
            public PresiPlayerGameModel Player { get; }
            public List<int> Cards = new List<int>();

            public int? Position = null;

            public List<int> Exhange= new List<int>();
            public List<int> ExhangeSelected = new List<int>();
            public List<int> NewCards = new List<int>();
            public int cardToChange = 0;

            public PlayerInGame(PresiPlayerGameModel p, Action<PresiGameModel> callBack) {
                Player = p;
                CallBack = callBack;
            }
        }

    }
}
