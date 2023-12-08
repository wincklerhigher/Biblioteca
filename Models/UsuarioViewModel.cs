using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Models
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Login é obrigatório.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmarSenha { get; set; }

        public string UsuarioLogado { get; set; }

        [Required(ErrorMessage = "O campo Tipo é obrigatório.")]
        [Display(Name = "Tipo")]
        public UsuarioTipo Tipo { get; set; }
        
        public List<SelectListItem> TiposDisponiveis { get; set; }

        public List<Usuario> Usuarios { get; set; }
 
        public UsuarioEditViewModel UsuarioAtual { get; set; }

        public class UsuarioEditViewModel
        {
            public int Id { get; set; } 
        }
    }
}