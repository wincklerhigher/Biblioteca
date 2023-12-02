using System;
using System.Collections.Generic;

namespace Biblioteca.Models
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Ano { get; set; }
        
        public static int TotalLivros { get; set; }

        public static List<Livro> GetLivrosPaginados(int page, int perPage)
        {
            var startIndex = (page - 1) * perPage;
            var endIndex = startIndex + perPage;
        
            var listaCompletaDeLivros = ObtenhaSuaListaCompletaDeLivros();

            TotalLivros = listaCompletaDeLivros.Count;

            var livrosPaginados = listaCompletaDeLivros.GetRange(startIndex, Math.Min(perPage, TotalLivros - startIndex));

            return livrosPaginados;
        }
        private static List<Livro> ObtenhaSuaListaCompletaDeLivros()
        {        
            return new List<Livro>    {               
            };
        }
    }
}