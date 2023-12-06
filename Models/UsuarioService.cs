using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;

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

        public Usuario ObterUsuarioPorTipo(UsuarioTipo tipo)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Tipo == tipo);
        }

       
        public void AdicionarUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public void AtualizarUsuario(Usuario usuario, string userRole)
    {
        if (userRole != "ADMIN")
        {
            throw new AccessDeniedException("Access denied");
        }

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
        
        public bool IsUsuarioAdmin(UsuarioTipo tipo)
{
    var usuario = ObterUsuarioPorTipo(tipo);

    // Verificar se o usuário é o usuário padrão ou admin
    if (usuario != null && (usuario.Tipo == UsuarioTipo.PADRAO || usuario.Tipo == UsuarioTipo.ADMIN))
    {
        return true;
    }

    return false;
}

        public bool AutenticarUsuario(string login, string senha)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Login == login && u.Senha == senha);
            
            if (usuario != null)
            {
                System.Diagnostics.Trace.WriteLine($"Usuário autenticado: {login}");
                return true;
            }
        
            System.Diagnostics.Trace.WriteLine($"Falha na autenticação de usuário: {login}");
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

        internal object ObterUsuarioPorId(string usuarioAtualId)
        {
            throw new NotImplementedException();
        }
    }
}