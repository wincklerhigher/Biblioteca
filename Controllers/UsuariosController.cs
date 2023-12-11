using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Biblioteca.Controllers
{        
    public class UsuariosController : Controller
    {        
        private readonly UsuarioService _usuarioService;        
        private readonly UserManager<IdentityUser> _userManager;

public UsuariosController(UsuarioService usuarioService, UserManager<IdentityUser> userManager)
{
    _usuarioService = usuarioService;
    _userManager = userManager;
}

        public IActionResult ListaDeUsuarios()
    {
    Autenticacao.CheckLogin(this);

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
        [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegistrarUsuarios(UsuarioViewModel usuario)
        {    
    if (User.IsInRole("Admin"))
    {
        if (ModelState.IsValid)
        {
            await _usuarioService.CriarNovoUsuario(usuario);

            return RedirectToAction("ListaDeUsuarios");
        }

        return View(usuario);
    }
    return RedirectToAction("AcessoNegado", "Usuarios");
    }

       [HttpPost]
[ValidateAntiForgeryToken]        
public async Task<IActionResult> Criar(Usuario usuario)
{
    if (ModelState.IsValid)
    {
        try
        {
            await _usuarioService.AdicionarUsuario(usuario);
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
    
    var tiposDisponiveis = Enum.GetValues(typeof(UsuarioTipo))
        .Cast<UsuarioTipo>()
        .Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() })
        .ToList();

    var viewModel = new UsuarioViewModel
    {
        Id = usuario.Id,
        Nome = usuario.Nome,
        Login = usuario.Login,
        Senha = usuario.Senha,
        Tipo = usuario.Tipo,
        TiposDisponiveis = tiposDisponiveis,
        UsuarioAtual = new UsuarioViewModel.UsuarioEditViewModel
        {
            Id = usuario.Id
        }
    };

    return View(viewModel);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Editar(UsuarioViewModel usuarioViewModel)
{
    try
    {        
        var tiposDisponiveis = Enum.GetValues(typeof(UsuarioTipo))
            .Cast<UsuarioTipo>()
            .Select(t => new SelectListItem { Value = t.ToString(), Text = t.ToString() })
            .ToList();

        usuarioViewModel.TiposDisponiveis = tiposDisponiveis;  

        var usuario = new Usuario
        {
            Id = usuarioViewModel.Id,
            Nome = usuarioViewModel.Nome,
            Login = usuarioViewModel.Login,
            Senha = usuarioViewModel.Senha,
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

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginViewModel model)
{    
    var autenticacaoValida = new AutenticacaoValida(); 
    if (autenticacaoValida.VerificarCredenciais(model)) 
    {
        Autenticacao.CheckLogin(this);
        var claims = new List<Claim>
        {
        new Claim(ClaimTypes.Name, model.UserName)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        ViewBag.IsRegistration = false;
        ViewBag.IsLoggedin = true; 

        return RedirectToAction("Index"); 
    }
    
    ModelState.AddModelError(string.Empty, "Credenciais inválidas");
    return View(model);
}

    [HttpGet]
    public IActionResult Logout()
{
    Autenticacao.CheckLogin(this);
    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return View();
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult LogoutConfirmado() 
{       
    return RedirectToAction("Login", "Home");
}
    }
}