using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Security
{
    public class JwToken
    {
        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";
        private IConfiguration _config;

        public JwToken(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user, int expireMinutes = 30)
        {
            //            var symmetricKey = Convert.FromBase64String(Secret);
            //            var tokenHandler = new JwtSecurityTokenHandler();
            //
            //            var now = DateTime.UtcNow;
            //            var tokenDescriptor = new SecurityTokenDescriptor
            //            {
            //                Subject = new ClaimsIdentity(new[]
            //                {
            //                    
            //                    new Claim(ClaimTypes.Email, user.Email),
            //                }),
            //
            //                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),
            //
            //                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            //            };
            //            return tokenHandler.CreateToken(tokenDescriptor);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var Subject = new List<Claim>(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            });
                       
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                Subject,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
