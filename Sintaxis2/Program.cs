using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintaxis2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Lenguaje l = new Lenguaje();

            /*while (!l.archivo.EndOfStream)
            {
                Console.WriteLine("Contenido: " + l.GetContenido());
                Console.WriteLine("Clasificación: " + l.CtoStr(l.GetClasificacion()));
                l.NextToken();
            }*/
            l.ProgramaC();
        }
    }
}
