using System;
using Biblioteca.Models;
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
            string hashSenha = Criptografo.TextoCriptografado(senha);

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