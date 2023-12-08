using System;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Biblioteca.Models;
using Microsoft.AspNetCore.Http;

namespace Biblioteca.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
    
        public IActionResult Index()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string senha, [FromServices] DbContextOptions<BibliotecaContext> options)
        {
    using (var contexto = new BibliotecaContext(options))
    {
        var usuario = contexto.Usuarios.FirstOrDefault(u => u.Login == login && u.Senha == senha);
        if (usuario == null)
        {
            ViewData["Erro"] = "Senha inválida";
            return View();
        }
        else
        {
            HttpContext.Session.SetString("user", usuario.Login);
            return RedirectToAction("Index");
        }
    }
}  
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
