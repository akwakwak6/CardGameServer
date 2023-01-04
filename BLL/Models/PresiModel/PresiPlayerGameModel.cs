
namespace BLL.Models.PresiModel {

    public enum PresiRoles {
        Neutre = 0,
        President = 1,
        VicePresident = 2,
        ViceBum = 3,
        Bum = 4
    }

    public class PresiPlayerGameModel {
        public int Id { get; set; }
        public string Pseudo { get; set; }
        public int NbCard { get; set; }
        public PresiRoles Role { get; set; }
        public bool Passed { get; set; }
        public Boolean IsPlaying { get; set; }

    }
}
