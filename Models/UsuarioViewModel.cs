using System.Collections.Generic;

namespace Biblioteca.Models

{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public int Tipo { get; set; }
        public string ConfirmarSenha { get; set; }
        public List<Usuario> Usuarios { get; set; }
        public Usuario UsuarioAtual { get; set; }
    }
}