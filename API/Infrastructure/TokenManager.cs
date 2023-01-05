using BLL.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace API.Infrastructure {
    public class TokenManager {

        private readonly string _secret;

        public TokenManager(IConfiguration config) {
            _secret = config.GetSection("TokenInfo").GetSection("secret").Value;
        }

        public string ReadToken(string token, string claimName) {
            //TODO check token
            /*var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;*/

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            return jwtSecurityToken.Claims.First(claim => claim.Type == claimName).Value;
        }

        public string GenerateToken(UserConnectedDalModel user) {
            if (user == null) throw new ArgumentNullException();

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            Claim[] claims = new Claim[] {
                new Claim("UserId",user.Id.ToString()),
                new Claim("Pseudo",user.Pseudo),
                new Claim(ClaimTypes.Role,"user")
            };

            JwtSecurityToken token = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: credentials,
                    expires: DateTime.Now.AddMinutes(60)
                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(token);
        }
    }
}
