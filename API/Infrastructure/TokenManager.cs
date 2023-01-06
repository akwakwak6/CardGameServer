using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using BLL.Models;

namespace API.Infrastructure {
    public class TokenManager {

        private readonly string _secret;

        public TokenManager(IConfiguration config) {
            _secret = config.GetSection("TokenInfo").GetSection("secret").Value ?? "P@ssw0rd";
        }

        public JwtSecurityToken? ReadToken(string token) {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secret);

            try {
                handler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken t);
                return t as JwtSecurityToken;
            } catch(Exception ex) {
                return null;
            }     

        }

        public int? GetUserId(string token) {
            if (int.TryParse(ReadToken(token)?.Claims.First(c => c.Type == "UserId").Value, out int o))
                return o;
            return null;
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
