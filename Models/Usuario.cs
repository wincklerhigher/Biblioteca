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
                // Lógica para executar a função do administrador
                // ...
            }
            else if (Tipo == UsuarioTipo.PADRAO)
            {
                // Lógica para executar a função do usuário padrão
                // ...
            }
            else
            {
                // Lógica para outros tipos de usuário, se necessário
                // ...
            }
        }
    }
}