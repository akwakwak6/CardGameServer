using DAL;
using Entities.Presi;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services {
    public static class InitDB {

        public static void CleanDB() {
            using (CardGameDbContext db = new CardGameDbContext()) {
                //db.PresiPlayers.Where(p => p.IsPlaying).ToList().ForEach(p => p.IsPlaying = false);
                List<PresiTable> tables = db.PresiTables.Include(t => t.Players.Where(p => p.IsPlaying)).Where(t => t.IsActive).ToList();

                tables.ForEach(t => {
                    Console.WriteLine("clean table "+t.Id + "  => " + t.Players.Count);
                    t.Players.ToList().ForEach(p => p.IsPlaying = false);
                    t.IsActive = false;
                });

                db.SaveChanges();
                Console.WriteLine("db cleaned");
            }
                
        }

    }
}
