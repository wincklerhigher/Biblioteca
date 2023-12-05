using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Biblioteca
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string senha = "senha123";
            string hashSenha = PasswordHasher.HashPassword(senha);

            Console.WriteLine("Senha: " + senha);
            Console.WriteLine("Hash MD5: " + hashSenha);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}