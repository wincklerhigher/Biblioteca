using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class UsuarioService
    {
         private readonly BibliotecaContext _context;

        public UsuarioService(BibliotecaContext context)
        {
            _context = context;
        }
        
        public List<Usuario> ObterTodosUsuarios()
        {
            return _context.Usuarios.ToList();
        }

        public Usuario ObterUsuarioPorLogin(string login)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Login == login);
        }

        public Usuario ObterUsuarioPorId(int id)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public void AdicionarUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public void AtualizarUsuario(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void RemoverUsuario(int usuarioId)
        {
            var usuario = _context.Usuarios.Find(usuarioId);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
        }

        public bool IsUsuarioAdmin(string login)
        {
            var usuario = ObterUsuarioPorLogin(login);
            return usuario != null && usuario.Tipo == UsuarioTipo.ADMIN;
        }

     public bool AutenticarUsuario(string login, string senha)
{
    var usuario = _context.Usuarios.FirstOrDefault(u => u.Login == login && u.Senha == senha);
    
    if (usuario != null)
    {
        System.Diagnostics.Trace.WriteLine($"Usuário autenticado: {login}");
        return true;
    }
 
        System.Diagnostics.Trace.WriteLine($"Falha na autenticação para o usuário: {login}");
        return false;
}
        public void CriarNovoUsuario(UsuarioViewModel usuarioViewModel)
        {   
            Usuario novoUsuario = new Usuario
            {
                Nome = usuarioViewModel.Nome,
                Login = usuarioViewModel.Login,
                Senha = usuarioViewModel.Senha,
                Tipo = usuarioViewModel.Tipo
            };

            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();
        }
    }
}