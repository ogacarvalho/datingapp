using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            this._context = context;
            this._tokenService = tokenService;
        }

        [HttpPost("register")] // POST: api/account/register // (AccountController + placeholder [-controller] == account)
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO) // Para realizar um POST de um objeto, os parâmetros devem ser um objeto.
        {   
            if (await UserExists(registerDTO.Username)) return BadRequest("Username already exists"); // Validação

            using var hmac = new HMACSHA512(); // Esse é o nosso "Salt" que fará a aleatorização do password do usuário. → "Using" assim que terminarmos de usar a classe queremos nos desfazer dela, o using faz isso. Avisa o garbage collector que essa classe pode ser liberada. [Necessário IDisposable]

            var user = new AppUser // Criando uma instância do usuário e atribuindo valores ao seus atributos.
            {
                UserName = registerDTO.Username.ToLower(), // É uma padrão para facilitar a vida. 
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)), // PasswordHash espera receber um array de bytes.
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user); // Aqui estamos apenas colocando no radar da entidade que o objeto usuários pode ser incluido na base.
            await _context.SaveChangesAsync(); // Agora sim estamos incluindo na base.

            return new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDTO.Username);

            if(user == null) return Unauthorized("Invalid username."); // O type ActionResult nos permite retornar métodos HTTPs como este <--.

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDTO
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower()); // Vai percorrer a tabela "Users" e dizer se a coluna UserName já possui tal nome através do AnyAsync (procura qlq coisa assync).
        }
    }
}