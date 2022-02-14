using System;
using System.Collections.Generic;

namespace CadastroUsuario
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public virtual List<Endereco> Enderecos{ get; set; }
    }
}
