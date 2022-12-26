

using BLL.Models;
using Entities;
using System.Runtime.CompilerServices;

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
