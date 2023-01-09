namespace Entities.Presi {
    public class PresiGame {

        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int PresiTableId { get; set; }
        public ICollection<PresiPlayer> PresiPlayers { get; set; }

    }
}
