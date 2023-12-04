using System.Collections.Generic;

namespace Biblioteca.Models
{
    public class CadEmprestimoViewModel
    {
        public ICollection<Livro> Livros { get; set; }
        public Emprestimo Emprestimo { get; set; }
        public List<Emprestimo> Emprestimos { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}