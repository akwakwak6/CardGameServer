using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Infrastructure {
    public class TokenManager {

        private readonly string _secret;

        public TokenManager(IConfiguration config) {
            _secret = config.GetSection("TokenInfo").GetSection("secret").Value;
        }

        public string GenerateToken(/*User user*/) {//TODO
            //if (user == null) throw new ArgumentNullException();

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            Claim[] claims = new Claim[] {
                /*new Claim(ClaimTypes.Role,user.IsAdmin ? "admin" : "user"),
                new Claim("email",user.Email),
                new Claim("UserId",user.Id.ToString())*/
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
