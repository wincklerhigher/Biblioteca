using System;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Controllers
{
    public class LivroController : Controller
    {
        private readonly LivroService _livroService;
        private readonly BibliotecaContext _context;

        public LivroController(LivroService livroService, BibliotecaContext context)
        {
            _livroService = livroService;
            _context = context;
        }

        private const int ItensPorPagina = 10;

        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Livro l)
        {
            if (l.Id == 0)
            {
                _livroService.Inserir(l);
            }
            else
            {
                _livroService.Atualizar(l);
            }

            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem(int page = 1, string tipoFiltro = "", string filtro = "")
        {
            Autenticacao.CheckLogin(this);

            FiltrosLivros objFiltro = !string.IsNullOrEmpty(filtro) ?
                new FiltrosLivros { Filtro = filtro, TipoFiltro = tipoFiltro } :
                null;

            var livros = _livroService.ListarTodos(objFiltro).ToList();
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
            Livro l = _livroService.ObterPorId(id);
            return View(l);
        }

        [HttpPost]
    public IActionResult Excluir(int id)
    {
        var livro = _context.Livros.Find(id);

        if (livro == null)
        {
            return NotFound(); 
        }

        _context.Livros.Remove(livro);
        _context.SaveChanges();

        return RedirectToAction("Listagem"); 
    }

     [HttpPost]
    public async Task<IActionResult> ExcluirConfirmado(int id)
    {
        var livro = await _context.Livros.FindAsync(id);

        if (livro == null)
        {
            return NotFound(); 
        }

        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();

        return RedirectToAction("Listagem"); 
}
    }
}