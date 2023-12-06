using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Biblioteca.Models
{
    public class AutenticacaoValida
    {

        public bool VerificarCredenciais(LoginViewModel model)
    {     
        return model.UserName == "seuUsuario" && model.Password == "suaSenha";
    }
        public static async Task AutenticarUsuario(HttpContext httpContext, string nomeUsuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nomeUsuario),                
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public static async Task DesautenticarUsuario(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }     
    }
}