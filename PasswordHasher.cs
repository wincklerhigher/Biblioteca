using System;
using System.Security.Cryptography;
using System.Text;

namespace Biblioteca.Models
{
    public static class Criptografo
    {
        public static string TextoCriptografado(string textoSemFormatacao)
        {
            MD5 MD5Hasher = MD5.Create();
            byte[] bytes = Encoding.Default.GetBytes(textoSemFormatacao);
            byte[] bytecriptografado = MD5Hasher.ComputeHash(bytes);

            StringBuilder SB = new StringBuilder();

            foreach (byte b in bytecriptografado)
            {
                string DebugB = b.ToString("x2");
                SB.Append(DebugB);
            }

            return SB.ToString();
        }
    }
}