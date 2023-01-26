using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            this._context = context;
        }

        [HttpPost("register")] // POST: api/account/register // (AccountController + placeholder [-controller] == account)
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto) // Para realizar um POST de um objeto, os parâmetros devem ser um objeto.
        {   
            if (await UserExists(registerDto.Username)) return BadRequest("Username already exists"); // Validação

            using var hmac = new HMACSHA512(); // Esse é o nosso "Salt" que fará a aleatorização do password do usuário. → "Using" assim que terminarmos de usar a classe queremos nos desfazer dela, o using faz isso. Avisa o garbage collector que essa classe pode ser liberada. [Necessário IDisposable]

            var user = new AppUser // Criando uma instância do usuário e atribuindo valores ao seus atributos.
            {
                UserName = registerDto.Username.ToLower(), // É uma padrão para facilitar a vida. 
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // PasswordHash espera receber um array de bytes.
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user); // Aqui estamos apenas colocando no radar da entidade que o objeto usuários pode ser incluido na base.
            await _context.SaveChangesAsync(); // Agora sim estamos incluindo na base.

            return user;
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower()); // Vai percorrer a tabela "Users" e dizer se a coluna UserName já possui tal nome através do AnyAsync (procura qlq coisa assync).
        }
    }
}