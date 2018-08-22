using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintaxis2
{
    class Sintaxis : Lexico
    {
        private bool disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Release managed resources.
                    Console.WriteLine("Fin de análisis sintáctico.");
                }
                // Release unmanaged resources.
                // Set large fields to null.
                // Call Dispose on your base class.
                disposed = true;
            }
            base.Dispose(disposing);
        }

        public Sintaxis()
        {
            NextToken();
        }

        public Sintaxis(String nombre) : base(nombre)
        {
            NextToken();
        }


        public void Match(String esperado)
        {
            try
            {
                if (GetContenido() == esperado)
                {
                    NextToken();
                }
                else
                {
                    throw new SyntaxException(esperado, this);
                }
            }
            catch (SyntaxException)
            {
                log.WriteLine("Error de sintaxis: Se espera un " + esperado);
                log.WriteLine(": Línea:" + GetRows() + ", Columna: " + GetCols() + "\n");
                this.Dispose();
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public void Match(T esperado)
        {
            try
            {
                if (GetClasificacion() == esperado)
                {
                    NextToken();
                }
                else
                {
                    throw new SyntaxException(esperado, this);
                }
            }
            catch (SyntaxException)
            {
                log.WriteLine("Error de sintaxis: Se espera un " + CtoStr(esperado));
                log.WriteLine(": Línea:" + GetRows() + ", Columna: " + GetCols() + "\n");
                this.Dispose();
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }

    class SyntaxException : Exception
    {
        public SyntaxException(T esperado, Lexico token)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error de sintaxis: Se espera un " + token.CtoStr(esperado));
            Console.WriteLine("Contenido: " + token.GetContenido());
            Console.WriteLine("Clasificación: " + token.CtoStr(token.GetClasificacion()));
            Console.WriteLine("Línea: " + token.GetRows() + " Columna: " + token.GetCols());
            Console.ForegroundColor = ConsoleColor.White;
        }
        public SyntaxException(String esperado, Lexico token)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error de sintaxis: Se espera un " + esperado);
            Console.WriteLine("Contenido: " + token.GetContenido());
            Console.WriteLine("Clasificación: " + token.CtoStr(token.GetClasificacion()));
            Console.WriteLine("Línea: " + token.GetRows() + " Columna: " + token.GetCols());
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
