
namespace Entities.Presi {
    public class PresiPlayedCards {

        public int Id { get; set; }
        public PresiPlayer Player { get; set; }
        public PresiGame Game { get; set; }
        public int Tour { get; set; }
        public int Card { get; set; }
    }
}
