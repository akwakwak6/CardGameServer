using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Presi {
    public class PresiTable {

        public int Id { get; set; }
        public ICollection<PresiPlayer> Players { get; set; } = new List<PresiPlayer>();
        public int? UserId { get; set; }

    }
}
