

namespace Entities {
    public class User {

        public int Id { get; set; }
        public string Pseudo { get; set; }
        public string Pwd { get; set; }
        public byte[] Salt { get; set; }

    }
}
