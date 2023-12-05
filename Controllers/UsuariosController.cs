using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Controllers
{
    public class UsuariosController : Controller
    {
          private readonly BibliotecaContext _context;
    private readonly UsuarioService _usuarioService;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(BibliotecaContext context, ILogger<UsuariosController> logger)
    {
        _context = context;
        _usuarioService = new UsuarioService(_context);
        _logger = logger;
    }
         public IActionResult ListaDeUsuarios()
    {
        var usuarios = _usuarioService.ObterTodosUsuarios();
        var viewModel = new UsuarioViewModel
        {
            Usuarios = usuarios
        };
        return View(viewModel);
    }

        public IActionResult Detalhes(int id)
        {
            var usuario = _usuarioService.ObterUsuarioPorId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

     [HttpGet]
    public IActionResult TipoDeUsuarios()
    {
        var viewModel = new UsuarioViewModel
        {
            TiposDisponiveis = ObterTiposSelectList()
        };

        return View(viewModel);
    }

[HttpPost]
public IActionResult RegistrarUsuarios(UsuarioViewModel usuario)
{
    if (ModelState.IsValid)
    {          
        _usuarioService.CriarNovoUsuario(usuario);
        
        return RedirectToAction("Sucesso");
    }
    
    return View(usuario);
}

        [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Criar(Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _usuarioService.AdicionarUsuario(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar usuário: {ex.Message}");
            }
        }

        return View(usuario);
    }

         [HttpGet]
    public IActionResult Editar(int id)
    {
        var usuario = _usuarioService.ObterUsuarioPorId(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

        [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(int id, Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _usuarioService.AtualizarUsuario(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao editar usuário: {ex.Message}");
            }
        }

        return View(usuario);
    }
        public IActionResult Excluir(int id)
        {
            var usuario = _usuarioService.ObterUsuarioPorId(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarExclusao(int id)
        {
            try
            {
                _usuarioService.RemoverUsuario(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao excluir usuário: {ex.Message}");                
            }

            return RedirectToAction(nameof(Index));
        }
     private List<SelectListItem> ObterTiposSelectList()
    {
        var tipos = Enum.GetValues(typeof(UsuarioTipo)).Cast<UsuarioTipo>().Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() }).ToList();
        return tipos;
    }

        [HttpGet]
public IActionResult Logout()
{
    return View();
}
        [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LogoutConfirmed()
    {
        HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
  }
}