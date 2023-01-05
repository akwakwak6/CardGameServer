

using BLL.Models;
using Entities;

namespace BLL.Mappers {
    public static class UserMapper {

        public static UserConnectedDalModel ToConnectedModel(this User user) {

            return new UserConnectedDalModel {
                Pseudo = user.Pseudo,
                Id = user.Id
            };

        }

    }
}
