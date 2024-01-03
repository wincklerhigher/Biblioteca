using System;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Biblioteca.Models
{
    public class BibliotecaContext : DbContext
    {

        private readonly IConfiguration _configuration;

        public BibliotecaContext(DbContextOptions<BibliotecaContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)      {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Tipo)
                .IsRequired()
                .HasMaxLength(30)
                .HasColumnName("Tipo")
                .HasConversion<string>();

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Livro)
                .WithMany()
                .HasForeignKey(e => e.LivroId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{            
    if (!optionsBuilder.IsConfigured)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseMySql(connectionString, mySqlOptions => 
        {
            mySqlOptions.ServerVersion(new Version(8, 0, 23), ServerType.MySql);
        });
    }
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
    }
}