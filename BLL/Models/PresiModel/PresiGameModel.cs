

namespace BLL.Models.PresiModel {
    public class PresiGameModel {
        public List<PresiPlayerGameModel> Players { get; set; }
        public List<int> CenterCarte { get; set; }
        public int Playing { get; set; }
        public List<int> MyHand { get; set; }   
    }
}
