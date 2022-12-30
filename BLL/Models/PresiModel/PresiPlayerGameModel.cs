
namespace BLL.Models.PresiModel {
    public class PresiPlayerGameModel {
        public int Id { get; set; }
        public string Pseudo { get; set; }
        public int NbCard { get; set; }
        public int Role { get; set; }
        public Boolean IsPlaying { get; set; }
    }
}
