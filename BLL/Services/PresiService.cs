using DAL;
using Entities.Presi;

namespace BLL.Services {
    public class PresiService {

        private CardGameDbContext _DB;
        public PresiService(CardGameDbContext db) {
            _DB = db;
        }

        public void CreateTable(int userId,string pseudo) { 
        
            PresiTable table = new PresiTable() {
                UserId = userId,
            };

            table.Players.Add(new PresiPlayer() { Pseudo = pseudo });

            _DB.Add(table);

            _DB.SaveChanges();

        }
    }
}
