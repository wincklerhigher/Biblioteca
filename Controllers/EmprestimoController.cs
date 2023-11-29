using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

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

        public IActionResult Listagem(string tipoFiltro, string filtro)
        {
            FiltrosEmprestimos objFiltro = null;
            if (!string.IsNullOrEmpty(filtro))
            {
                objFiltro = new FiltrosEmprestimos();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;
            }
            EmprestimoService emprestimoService = new EmprestimoService();
            return View(emprestimoService.ListarTodos(objFiltro));
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