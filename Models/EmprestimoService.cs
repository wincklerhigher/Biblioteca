using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class EmprestimoService
    {
        private readonly BibliotecaContext _context;

        public EmprestimoService(BibliotecaContext context)
        {
            _context = context;
        }

        public void Inserir(Emprestimo e)
        {
            _context.Emprestimos.Add(e);
            _context.SaveChanges();
        }

        public void Atualizar(Emprestimo e)
        {
            Emprestimo emprestimo = _context.Emprestimos.Find(e.Id);
            emprestimo.NomeUsuario = e.NomeUsuario;
            emprestimo.Telefone = e.Telefone;
            emprestimo.LivroId = e.LivroId;
            emprestimo.DataEmprestimo = e.DataEmprestimo;
            emprestimo.DataDevolucao = e.DataDevolucao;

            _context.SaveChanges();
        }

        public List<Emprestimo> ListarTodosComDestaque(FiltrosEmprestimos filtro)
        {
            IQueryable<Emprestimo> query = _context.Emprestimos.Include(e => e.Livro);

            if (filtro != null)
            {
                switch (filtro.TipoFiltro)
                {
                    case "Usuario":
                        query = query.Where(e => e.NomeUsuario.Contains(filtro.Filtro));
                        break;

                    case "Livro":
                        if (!string.IsNullOrEmpty(filtro.Filtro) && int.TryParse(filtro.Filtro, out var livroId))
                        {
                            query = query.Where(e => e.LivroId == livroId);
                        }
                        break;                  

                    default:
                        
                        break;
                }
            }

            var emprestimos = query.ToList();

            foreach (var e in emprestimos)
            {
                e.Estado = e.ObterClasseDestaque();
            }

            return emprestimos;
        }

        public Emprestimo ObterPorId(int id)
        {
            return _context.Emprestimos.Find(id);
        }

        public void Apagar(int id)
        {
            Emprestimo emprestimo = _context.Emprestimos.Find(id);

            if (emprestimo != null)
            {
                _context.Emprestimos.Remove(emprestimo);
                _context.SaveChanges();
            }
        }
    }
}