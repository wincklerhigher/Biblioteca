using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biblioteca.Models
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Ano { get; set; }

        public static int TotalLivros { get; set; }

        public static List<Livro> GetLivrosPaginados(int page, int perPage, BibliotecaContext context)
        {
            var startIndex = (page - 1) * perPage;
            var endIndex = startIndex + perPage;

            var listaCompletaDeLivros = ObtenhaSuaListaCompletaDeLivros(context);

            TotalLivros = listaCompletaDeLivros.Count;

            var livrosPaginados = listaCompletaDeLivros.GetRange(startIndex, Math.Min(perPage, TotalLivros - startIndex));

            return livrosPaginados;
        }

        public async Task Excluir(BibliotecaContext context)
        {
            LivroService service = new LivroService(context);
            await service.ExcluirAsync(this.Id);
        }

        private static List<Livro> ObtenhaSuaListaCompletaDeLivros(BibliotecaContext context)
        {
            LivroService service = new LivroService(context);
            return service.ListarTodos() as List<Livro>; // Explicit cast to List<Livro>
        }
    }
}