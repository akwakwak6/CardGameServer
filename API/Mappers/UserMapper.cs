using API.Models;
using BLL.Models;

namespace API.Mappers {
    public static class UserMapper {
        
        public static ConnectedUserModel ToAPIuser(this UserConnectedDalModel user,string token = null) {
            return new ConnectedUserModel() { 
                Id = user.Id,
                Pseudo = user.Pseudo,
                Token = token
            };
        }

    }
}
