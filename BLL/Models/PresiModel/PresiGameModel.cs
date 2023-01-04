

namespace BLL.Models.PresiModel {
    public class PresiGameModel {
        public List<PresiPlayerGameModel> Players { get; set; }
        public List<int> CenterCarte { get; set; }
        public List<int> MyHand { get; set; }
        public Boolean ShowReady { get; set; }
        public PresiPlayerGameModel Me { get; set; }
        public List<int> ChangeCards { get; set; }
        public List<int> NewCards { get; set; }
    }
}
