using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semantica
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lenguaje L = new Lenguaje("prueba.cpp"))
                {
                    
                    L.Programa();
                    /*L.match(Token.Tipos.Numero);
                    //L.match("+");
                    L.match(Token.Tipos.OpTermino);
                    L.match(Token.Tipos.Identificador);
                    L.match(";");
                    */
                    /*
                    while(!L.finArchivo())
                    {
                        L.nextToken();
                    }
                    */
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
