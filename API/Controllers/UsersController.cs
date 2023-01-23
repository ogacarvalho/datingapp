using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {   
        private readonly DataContext _context;
        public UsersController(DataContext context) // Instância do DBContext criado durante a requisição. (Lifetime Session Scoped)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() // Retorna uma lista de AppUser
        {
            var users = await _context.Users.ToListAsync(); // Estamos acessando a sessão (_context) e em seguida acessando a nossa tabela criada "Users" e em seguida através do "ToList()" estamos solicitando que nos retorne a lista de dados desta tabela.
            return users;
        }

        [HttpGet("{id}")] // Não esquecer do parâmetro no cabeçalho.
        public async Task <ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }  
}