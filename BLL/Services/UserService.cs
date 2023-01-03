using BLL.Mappers;
using BLL.Models;
using DAL;
using Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services {
    public class UserService {

        private CardGameDbContext _DB;
        public UserService(CardGameDbContext db) {
            _DB = db;
        }

        private string HashPwd(string password,out byte[] salt) {

            salt = RandomNumberGenerator.GetBytes(128 / 8);

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }

        private string HashPwd(string password, byte[] salt) {

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }

        public UserConnectedDalModel Login(UserLoginModel user) {


            //TODO hash pwd in DB

            User? u = _DB.Users.Where(b => b.Pseudo == user.pseudo).SingleOrDefault();//TODO pseudo is unique

            if( u is null ) throw new Exception("Error login");

            string pwd = HashPwd(user.Pwd, u.Salt);

            if (pwd == u.Pwd)
                return u.ToConnectedModel();

            throw new Exception("Error pwd");


        }

        public UserConnectedDalModel Register(UserRegisterModel user) {

            string hashed = HashPwd(user.Pwd,out byte[] salt);

            User userDB = new User() {
                Pwd = hashed,
                Pseudo = user.pseudo,
                Salt= salt,
            };

            _DB.Add(userDB);

            _DB.SaveChanges();

            return userDB.ToConnectedModel();


        }
    }
}
