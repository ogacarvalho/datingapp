using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {

        /* Existem dois tipos de chaves de criptografia: 
        Simétrica, a mesma chave é utilizada para Encrypting e Decrypting e tudo é feito no Servidor, ou seja o Client não faz nada.
        Asimétrica, quando o server precisa fazer o Encrypting e o Client o Decrypting, temos neste caso uma chave publica e uma chave privada. (Exemplo: SSL e HTTP)
        */
        private readonly SymmetricSecurityKey _key; 


        public TokenService(IConfiguration config)
        {
         _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])); // A classe recebe o tipo "byte[]" então por isso precisa do "Encoding.UTF8..."
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName) // São informações que o usuário pode "AFIRMAR".
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); // O segundo parâmetro é o tipo de algorítimo que queremos usar para criar a credencial.

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}