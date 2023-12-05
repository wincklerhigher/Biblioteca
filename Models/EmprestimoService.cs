using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;

namespace Biblioteca.Models
{
    public class EmprestimoService 
    {
        public void Inserir(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Emprestimos.Add(e);
                bc.SaveChanges();
            }
        }

        public void Atualizar(Emprestimo e)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                Emprestimo emprestimo = bc.Emprestimos.Find(e.Id);
                emprestimo.NomeUsuario = e.NomeUsuario;
                emprestimo.Telefone = e.Telefone;
                emprestimo.LivroId = e.LivroId;
                emprestimo.DataEmprestimo = e.DataEmprestimo;
                emprestimo.DataDevolucao = e.DataDevolucao;

                bc.SaveChanges();
            }
        }
 public List<Emprestimo> ListarTodosComDestaque(FiltrosEmprestimos filtro)
{
    using (BibliotecaContext bc = new BibliotecaContext())
    {
        IQueryable<Emprestimo> query = bc.Emprestimos.Include(e => e.Livro);

        if (filtro != null)
        {            
            switch (filtro.TipoFiltro)
            {
                case "Usuario":
                    query = query.Where(e => e.NomeUsuario.Contains(filtro.Filtro));
                    break;

                case "Livro":
                    if (!string.IsNullOrEmpty(filtro.Filtro))
                    {
                        int livroId;
                        if (int.TryParse(filtro.Filtro, out livroId))
                        {
                            query = query.Where(e => e.LivroId == livroId);
                        }
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
}
public Emprestimo ObterPorId(int id)
{
    using(BibliotecaContext bc = new BibliotecaContext())
    {
        return bc.Emprestimos.Find(id);
    }
}
        public void Apagar(int id)
        {
            using (BibliotecaContext bc = new BibliotecaContext())
            {
                Emprestimo emprestimo = bc.Emprestimos.Find(id);

                if (emprestimo != null)
                {
                    bc.Emprestimos.Remove(emprestimo);
                    bc.SaveChanges();
                }
            }
        }
    }
}