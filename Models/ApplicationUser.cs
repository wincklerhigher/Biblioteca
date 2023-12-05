using Microsoft.AspNetCore.Identity;

namespace Biblioteca.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nome { get; set; }
        public UsuarioTipo Tipo { get; set; }
    }
}