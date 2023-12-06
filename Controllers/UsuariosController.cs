using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Biblioteca.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

      public IActionResult ListaDeUsuarios()
{
    var usuarios = _usuarioService.ObterTodosUsuarios();
    var viewModel = new UsuarioViewModel
    {
        Usuarios = usuarios
    };
    return View("ListaDeUsuarios", viewModel);
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

                return RedirectToAction("ListaDeUsuarios");
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
                    Debug.WriteLine($"Erro ao criar usuário: {ex.Message}");
                }
            }

            return View(usuario);
        }

          public IActionResult AcessoNegado()
    {
        return View();
    }   

        public IActionResult Editar(int id)
{
    var usuario = _usuarioService.ObterUsuarioPorId(id);

    if (usuario == null)
    {
        return NotFound();
    }

    var viewModel = new UsuarioViewModel
    {
        Usuarios = new List<Usuario> { usuario }
    };

    return View(viewModel);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Editar(UsuarioViewModel usuarioViewModel)
{
    try
    {        
        var usuarioAtualId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Verificar se o usuário atual é um administrador
        if (!User.IsInRole("ADMIN"))
        {
            return RedirectToAction("AcessoNegado", "Usuarios");
        }
    
        var usuario = new Usuario
        {
            Id = usuarioViewModel.Id,
            Nome = usuarioViewModel.Nome,
            Tipo = usuarioViewModel.Tipo

        };
        
        _usuarioService.AtualizarUsuario(usuario, usuario.Tipo.ToString());

        return RedirectToAction(nameof(ListaDeUsuarios));
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", $"Erro ao editar usuário: {ex.Message}");
    }

    return View(usuarioViewModel);
}
       
    public IActionResult Excluir(int id)
{
    var usuario = _usuarioService.ObterUsuarioPorId(id);

    if (usuario == null)
    {
        return NotFound();
    } 

    // Verificar se o usuário atual é um administrador
    if (usuario.Tipo != UsuarioTipo.PADRAO)
    {
        return RedirectToAction("AcessoNegado", "Usuarios");
    }

    var viewModel = new UsuarioViewModel
    {
        Usuarios = new List<Usuario> { usuario }
    };

    return View(viewModel);
}

        [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult ConfirmarExclusao(int id)
{
    try
    {
        var usuario = _usuarioService.ObterUsuarioPorId(id);

        // Verificar se o usuário é do tipo PADRAO
        if (usuario.Tipo != UsuarioTipo.PADRAO)
        {
            return RedirectToAction("AcessoNegado", "Usuarios");
        }

        _usuarioService.RemoverUsuario(id);
        return RedirectToAction(nameof(ListaDeUsuarios));
    }
    catch (Exception ex)
    {
        ModelState.AddModelError("", $"Erro ao excluir usuário: {ex.Message}");
    }

    return RedirectToAction(nameof(ListaDeUsuarios));
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
        public IActionResult LogoutConfirmado()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }
    }
}