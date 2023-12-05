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
        private readonly UsuarioService _usuarioService;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(UsuarioService usuarioService, ILogger<UsuariosController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }
        public IActionResult ListaDeUsuarios()
        {
            var model = new UsuarioViewModel();

            return View(model);
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

     public IActionResult TipoDeUsuarios()
{
    var viewModel = new UsuarioViewModel
    {
        TiposDisponiveis = new List<SelectListItem>
        {
            new SelectListItem { Value = UsuarioTipo.ADMIN.ToString(), Text = UsuarioTipo.ADMIN.ToString() },
            new SelectListItem { Value = UsuarioTipo.PADRAO.ToString(), Text = UsuarioTipo.PADRAO.ToString() }
        }
    };

    return View(viewModel);
}

public IActionResult RegistrarUsuarios()
{   
    List<UsuarioTipo> tipos = Enum.GetValues(typeof(UsuarioTipo)).Cast<UsuarioTipo>().ToList();
    
    List<SelectListItem> tiposSelectList = tipos
        .Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() })
        .ToList();

    UsuarioViewModel viewModel = new UsuarioViewModel
    {
        TiposDisponiveis = tiposSelectList
    };

    return View(viewModel);
}

        public IActionResult Criar()
        {
            return View();
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