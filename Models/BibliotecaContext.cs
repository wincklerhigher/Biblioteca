using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Biblioteca.Models
{
    public class BibliotecaContext : DbContext
    {
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            string connectionString = "Server=localhost;Database=Biblioteca;User=root;Password=;";

            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 23)));
        }

        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Senha)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }        

        // Método para criar um hash MD5 da senha do usuário
        private string CriarHashMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public void SeedData()
        {
        }
    }
}