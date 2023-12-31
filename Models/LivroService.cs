using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biblioteca.Models
{
    public class LivroService
    {
        private readonly BibliotecaContext _context;

        public LivroService(BibliotecaContext context)
        {
            _context = context;
        }

        public void Inserir(Livro l)
        {
            _context.Livros.Add(l);
            _context.SaveChanges();
        }

        public void Atualizar(Livro l)
        {
            Livro livro = _context.Livros.Find(l.Id);
            if (livro != null)
            {
                livro.Autor = l.Autor;
                livro.Titulo = l.Titulo;
                _context.SaveChanges();
            }
        }

        public async Task ExcluirAsync(int id)
{
    Livro livro = await _context.Livros.FindAsync(id);
    if (livro != null)
    {
        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();
    }
}

        public ICollection<Livro> ListarTodos(FiltrosLivros filtro = null)
        {
            IQueryable<Livro> query = _context.Livros;

            if (filtro != null)
            {
                switch (filtro.TipoFiltro)
                {
                    case "Autor":
                        query = query.Where(l => l.Autor.Contains(filtro.Filtro));
                        break;

                    case "Titulo":
                        query = query.Where(l => l.Titulo.Contains(filtro.Filtro));
                        break;                   
                }
            }

            return query.OrderBy(l => l.Titulo).ToList();
        }

        public ICollection<Livro> ListarDisponiveis()
        {
            return _context.Livros.ToList();
        }

        public Livro ObterPorId(int id)
        {
            return _context.Livros.Find(id);
        }
    }
}