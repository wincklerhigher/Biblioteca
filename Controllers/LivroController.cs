using System;
using System.Collections.Generic;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Biblioteca.Controllers
{
    public class LivroController : Controller
    {
        private const int ItensPorPagina = 10;

        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Livro l)
        {
            LivroService livroService = new LivroService();

            if (l.Id == 0)
            {
                livroService.Inserir(l);
            }
            else
            {
                livroService.Atualizar(l);
            }

            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem(int page = 1, string tipoFiltro = "", string filtro = "")
        {
            Autenticacao.CheckLogin(this);

            FiltrosLivros objFiltro = !string.IsNullOrEmpty(filtro) ?
                new FiltrosLivros { Filtro = filtro, TipoFiltro = tipoFiltro } :
                null;

            LivroService livroService = new LivroService();

            var livros = livroService.ListarTodos(objFiltro).ToList();
            var totalPaginas = (int)Math.Ceiling((double)livros.Count / ItensPorPagina);

            page = Math.Max(1, Math.Min(page, totalPaginas));

            var startIndex = (page - 1) * ItensPorPagina;
            var livrosPaginados = livros.Skip(startIndex).Take(ItensPorPagina).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPaginas;

            return View(livrosPaginados);
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLogin(this);
            LivroService ls = new LivroService();
            Livro l = ls.ObterPorId(id);
            return View(l);
        }
    }
}