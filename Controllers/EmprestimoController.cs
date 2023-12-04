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
    if (!ModelState.IsValid)
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
     
public IActionResult ListarEmprestimos()
{
    EmprestimoService emprestimoService = new EmprestimoService();
    var emprestimosComDestaque = emprestimoService.ListarTodosComDestaque(null);
    return View(emprestimosComDestaque);
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
    var emprestimosComDestaque = emprestimoService.ListarTodosComDestaque(objFiltro);

    var emprestimosPaginados = emprestimosComDestaque
        .OfType<Emprestimo>()
        .Skip((page - 1) * perPage)
        .Take(perPage)
        .ToList();

    var totalPages = (int)Math.Ceiling((double)emprestimosComDestaque.Count / perPage);

    var viewModel = new CadEmprestimoViewModel
    {
        Emprestimos = emprestimosPaginados,
        CurrentPage = page,
        TotalPages = totalPages
    };

    return View(viewModel);
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

        [HttpPost]
        public IActionResult ConfirmarApagar(int id)
        {
            EmprestimoService emprestimoService = new EmprestimoService();
            Emprestimo emprestimo = emprestimoService.ObterPorId(id);

            if (emprestimo == null)
            {
                return NotFound(); 
            }

            emprestimoService.Apagar(id);

            return RedirectToAction("Listagem");
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