using System;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Controllers
{
    public class EmprestimoController : Controller
    {
        private readonly EmprestimoService _emprestimoService;
        private readonly LivroService _livroService;

        public EmprestimoController(EmprestimoService emprestimoService, LivroService livroService)
        {
            _emprestimoService = emprestimoService;
            _livroService = livroService;
        }
        
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = _livroService.ListarTodos();

            return View(cadModel);
        }

        [HttpPost]
        public IActionResult Cadastro(CadEmprestimoViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Livros = _livroService.ListarTodos();
                return View(viewModel);
            }

            if (viewModel.Emprestimo.Id == 0)
            {
                _emprestimoService.Inserir(viewModel.Emprestimo);
            }
            else
            {
                _emprestimoService.Atualizar(viewModel.Emprestimo);
            }

            return RedirectToAction("Listagem");
        }

        public IActionResult ListarEmprestimos()
        {
            var emprestimosComDestaque = _emprestimoService.ListarTodosComDestaque(null);
            return View(emprestimosComDestaque);
        }
        
        public IActionResult Listagem(int page = 1, string tipoFiltro = "", string filtro = "")
        {

            Autenticacao.CheckLogin(this);
            
            FiltrosEmprestimos objFiltro = !string.IsNullOrEmpty(filtro) ?
                new FiltrosEmprestimos { Filtro = filtro, TipoFiltro = tipoFiltro } :
                null;

            const int perPage = 10;
            var emprestimosComDestaque = _emprestimoService.ListarTodosComDestaque(objFiltro);

            var emprestimosFiltrados = emprestimosComDestaque
                .OfType<Emprestimo>()
                .Where(e => e.NomeUsuario != null && e.NomeUsuario.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) != -1 ||
                            e.Livro != null && e.Livro.Titulo != null && e.Livro.Titulo.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) != -1)
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)emprestimosFiltrados.Count / perPage);

            var viewModel = new CadEmprestimoViewModel
            {
                Emprestimos = emprestimosFiltrados,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        public IActionResult Edicao(int id)
        {
            Emprestimo e = _emprestimoService.ObterPorId(id);

            CadEmprestimoViewModel cadModel = new CadEmprestimoViewModel();
            cadModel.Livros = _livroService.ListarTodos();
            cadModel.Emprestimo = e;

            return View(cadModel);
        }

        public IActionResult Apagar(int id)
        {
            Emprestimo emprestimo = _emprestimoService.ObterPorId(id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        [HttpPost]
        public IActionResult ConfirmarApagar(int id)
        {
            Emprestimo emprestimo = _emprestimoService.ObterPorId(id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            _emprestimoService.Apagar(id);

            return RedirectToAction("Listagem");
        }

        [HttpPost]
        public IActionResult ApagarConfirmado(int id)
        {
            _emprestimoService.Apagar(id);

            return RedirectToAction("Listagem");
        }
    }
}