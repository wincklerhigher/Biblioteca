using System.Collections.Generic;

namespace Biblioteca.Models
{    
    public enum UsuarioTipo
    {
        ADMIN,
        PADRAO
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }       
        public UsuarioTipo Tipo { get; set; }

        // Verificar permissões antes de executar uma função
        public void ExecutarFuncao(string funcao)
        {
            if (Tipo == UsuarioTipo.ADMIN)
            {
                
            }
            else if (Tipo == UsuarioTipo.PADRAO)
            {
                
            }
            else
            {
                
            }
        }
    }
}