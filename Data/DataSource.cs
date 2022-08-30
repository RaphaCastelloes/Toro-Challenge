using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    // Representation of the datasource
    public class DataSource
    {
        public static List<Usuario> Usuario = new List<Usuario>()
        {
            new Usuario()
            {
                Id = 1,
                NumeroConta = "123456-01",
                Saldo = 1000000.00,
                CPF = "12345678901",
                Email = "castelloesjf@hotmail.com",
                Senha = "password",
            },
            new Usuario()
            {
                Id = 2,
                NumeroConta = "123456-02",
                Saldo = 1000,
                CPF = "12345678902",
                Email = "castelloesjf@gmail.com",
                Senha = "password",
            },
        };
    }
}