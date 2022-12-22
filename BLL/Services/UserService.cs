using BLL.Models;

namespace BLL.Services {
    public class UserService {


        public UserConnectedModel Login(UserLoginModel user) {
            return new UserConnectedModel { Id = 53 , Pseudo = user.pseudo, Token = "159"};

        }

        public UserConnectedModel Register(UserRegisterModel user) {
            return new UserConnectedModel { Id = 54, Pseudo = user.pseudo, Token = "160" };
        }
    }
}
