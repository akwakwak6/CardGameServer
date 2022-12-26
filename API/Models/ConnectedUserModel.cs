using BLL.Models;

namespace API.Models {
    public class ConnectedUserModel : UserConnectedDalModel {

        public string Token { get; set; }

    }
}
