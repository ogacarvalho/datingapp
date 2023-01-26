using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required] // Isso cria uma validação para o value atribuído, por exemplo "" não será aceito.
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}