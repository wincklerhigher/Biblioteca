using System;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace Biblioteca.Controllers
{
    public class EmprestimoController : Controller
    {
        public IActionResult Cadastro()
        {
            LivroService livroService = new LivroService();
            EmprestimoService emprestimoService = new EmprestimoService();

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = livroService.ListarTodos();
            return View(cadModel);
        }

        [HttpPost]
        public IActionResult Cadastro(CadEmprestimoViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Emprestimo.NomeUsuario) ||
                string.IsNullOrWhiteSpace(viewModel.Emprestimo.Telefone) ||
                viewModel.Emprestimo.DataEmprestimo == null ||
                viewModel.Emprestimo.DataDevolucao == null)
            {
                return View(viewModel);
            }

            EmprestimoService emprestimoService = new EmprestimoService();

            if (viewModel.Emprestimo.Id == 0)
            {
                emprestimoService.Inserir(viewModel.Emprestimo);
            }
            else
            {
                emprestimoService.Atualizar(viewModel.Emprestimo);
            }

            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem(int page = 1, string tipoFiltro = "", string filtro = "")
        {
            FiltrosEmprestimos objFiltro = null;
            if (!string.IsNullOrEmpty(filtro))
            {
                objFiltro = new FiltrosEmprestimos();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }

            const int perPage = 10;
            EmprestimoService emprestimoService = new EmprestimoService();

            var emprestimos = emprestimoService.ListarTodos(objFiltro).ToList();
            var totalPages = (int)Math.Ceiling((double)emprestimos.Count / perPage);

            page = Math.Max(1, Math.Min(page, totalPages));

            var startIndex = (page - 1) * perPage;
            var emprestimosPaginados = emprestimos.Skip(startIndex).Take(perPage).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(emprestimosPaginados);
        }

        public IActionResult Edicao(int id)
        {
            LivroService livroService = new LivroService();
            EmprestimoService em = new EmprestimoService();
            Emprestimo e = em.ObterPorId(id);

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = livroService.ListarTodos();
            cadModel.Emprestimo = e;

            return View(cadModel);
        }

        public IActionResult Apagar(int id)
        {
            EmprestimoService emprestimoService = new EmprestimoService();
            Emprestimo emprestimo = emprestimoService.ObterPorId(id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        public IActionResult ConfirmarApagar(int id)
        {
            EmprestimoService emprestimoService = new EmprestimoService();
            Emprestimo emprestimo = emprestimoService.ObterPorId(id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        [HttpPost]
        public IActionResult ApagarConfirmado(int id)
        {
            EmprestimoService emprestimoService = new EmprestimoService();
            emprestimoService.Apagar(id);

            return RedirectToAction("Listagem");
        }
    }
}