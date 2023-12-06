using System;

namespace Biblioteca.Models
{
    public enum UsuarioTipo
    {
        ADMIN,
        PADRAO
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public UsuarioTipo Tipo { get; set; }
        public bool IsAdmin => Tipo == UsuarioTipo.ADMIN;

         }
    }