using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;    
using System;
using System.Threading.Tasks;

namespace Biblioteca.Models
{
    public class UsuarioService
    {
        private readonly BibliotecaContext _context;
        private readonly UserManager<IdentityUser> _userManager;

    public UsuarioService(BibliotecaContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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
    
    if (userRole == "ADMIN" || userRole == "PADRAO")
    {
        _context.Entry(usuario).State = EntityState.Modified;
        _context.SaveChanges();
    }
    else
    {
        throw new AccessDeniedException("Access denied");
    }
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

  public async Task CriarNovoUsuario(UsuarioViewModel usuarioViewModel)
{   
    var novoUsuario = new IdentityUser
    {
        UserName = usuarioViewModel.Login,
        // Outras propriedades do IdentityUser, se necessário
    };

    var result = await _userManager.CreateAsync(novoUsuario, usuarioViewModel.Senha);

    if (result.Succeeded)
    {
        await _userManager.AddToRoleAsync(novoUsuario, "Padrao");
    }
    else
    {
        foreach (var error in result.Errors)
        {
            System.Diagnostics.Trace.WriteLine($"Erro ao criar usuário: {error.Description}");        }
    }
}
    }
}