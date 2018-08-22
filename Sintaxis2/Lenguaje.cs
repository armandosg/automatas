using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintaxis2
{
    class Lenguaje : Sintaxis
    {
        private bool disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Release managed resources.
                    Console.WriteLine("Fin de compilación.");
                }
                // Release unmanaged resources.
                // Set large fields to null.
                // Call Dispose on your base class.
                disposed = true;
            }
            base.Dispose(disposing);
        }

        public Lenguaje()
        {
        }

        public Lenguaje(String nombre) : base(nombre)
        {
        }

        public void ProgramaC()
        {
            Librerias();
            Namespace();
            Console.ReadLine();
        }

        private void Librerias()
        {
            Match("using");
            Libreria();
            Match(";");
            if (GetContenido() == "using")
            {
                Librerias();
            }
        }

        private void Libreria()
        {
            Match(T.IDENTIFICADOR);
            if (GetContenido() == ".")
            {
                Match(".");
                Libreria();
            }
        }

        private void Namespace()
        {
            Match("namespace");
            Match(T.IDENTIFICADOR);
            Match(T.INICIO_BLOQUE);
            Class();
            Match("}");
        }

        private void Class()
        {
            Match("class");
            Match("Program");
            Match(T.INICIO_BLOQUE);
            if (GetClasificacion() == T.TIPO_DATO || GetClasificacion() == T.TIPO_ZONA || GetClasificacion() == T.CONSTANTE)
            {
                Atributos();
            }
            Main();
            Match("}");
        }

        private void Main()
        {
            Match("static");
            Match("void");
            Match("Main");
            Match("(");
            Match(T.TIPO_DATO);
            Match("[");
            Match("]");
            Match("args");
            Match(")");
            BloqueDeInstrucciones();
        }
        

        private void Atributos()
        {
            if (GetClasificacion() == T.TIPO_ZONA)
            {
                Match(T.TIPO_ZONA);
            }
            if (GetClasificacion() == T.CONSTANTE)
            {
                Match(T.CONSTANTE);
            }
            Match(T.TIPO_DATO);
            ListID();
            Match(T.FIN_SENTENCIA);
            if (GetClasificacion() == T.TIPO_ZONA || GetClasificacion() == T.TIPO_DATO || GetClasificacion() == T.CONSTANTE)
            {
                Atributos();
            }
        }

        private void ListID()
        {
            Match(T.IDENTIFICADOR);
            if (GetContenido() == ",")
            {
                Match(",");
                ListID();
            }
        }

        private void BloqueDeInstrucciones()
        {
            Match(T.INICIO_BLOQUE);
            Instrucciones();
            Match(T.FIN_BLOQUE);
        }

        private void Instrucciones()
        {
            if (GetClasificacion() != T.FIN_BLOQUE)
            {
                Instruccion();
                if (GetClasificacion() != T.FIN_BLOQUE)
                {
                    Instrucciones();
                }
            }
        }
        private void Instruccion()
        {
            if (GetContenido() == "Console")
            {
                Match(GetContenido());
                Match(".");
                switch (GetContenido())
                {
                    case "WriteLine":
                        Match(GetContenido());
                        Match("(");
                        if (GetClasificacion() == T.CADENA)
                        {
                            Console.WriteLine(GetContenido().Substring(1, GetContenido().Length - 2));
                            Match(T.CADENA);
                        }
                        else
                        {
                            Expresion();
                        }
                        Match(")");
                        break;
                    case "ReadLine":
                        Match("ReadLine");
                        Match("(");
                        Match(")");
                        Console.ReadLine();
                        break;
                    default:
                        Match("ReadKey");
                        Match("(");
                        Match(")");
                        Console.ReadKey();
                        break;
                }
                Match(T.FIN_SENTENCIA);
            }
            else if (GetContenido() == "for")
            {
                For();
            }
            else if (GetContenido() == "if")
            {
                If();
            }
            else if(GetClasificacion() == T.IDENTIFICADOR) // Asignación radio = (5 + 3) * 8 (2 - 6) /2;
            {
                Match(T.IDENTIFICADOR);
                if (GetClasificacion() == T.ASIGNACION)
                {
                    Match(T.ASIGNACION);
                    if (GetContenido() == "Console")
                    {
                        Match("Console");
                        Match(".");
                        Match("ReadLine");
                        Match("(");
                        Match(")");
                        Console.ReadLine();
                    }
                    else
                    {
                        log.WriteLine(GetContenido() + " = ");
                        Expresion();
                    }
                }
                else if (GetClasificacion() == T.INCREMENTO_TERMINO)
                {
                    Match(T.INCREMENTO_TERMINO);
                }
                Match(T.FIN_SENTENCIA);
            }
            else
            {
                Match(T.FIN_SENTENCIA);
            }
        }

        private void Expresion() // Aquí va el código para reconocer: (5 + 3) * 8 (2 - 6) /2
        {
            Termino();
            MasTermino();
        }

        private void MasTermino() // -> (operadorTermino termino) ?
        {
            if (GetClasificacion() == T.OPERADOR_TERMINO)
            {
                String operado = GetContenido();
                Match(T.OPERADOR_TERMINO);
                Termino();
                Console.WriteLine(GetContenido() + " ")
            }
        }

        private void Termino() // -> Factor PorFactor
        {
            Factor();
            PorFactor();
        }

        private void PorFactor() // -> (operadorFactor Factor) ?
        {
            if (GetClasificacion() == T.OPERADOR_FACTOR)
            {
                String operador = GetContenido();
                Match(T.OPERADOR_FACTOR);
                Factor();
                log.WriteLine(GetContenido() + " ");
            }
        }

        private void Factor()
        {
            if (GetClasificacion() == T.NUMERO)
            {
                Console.WriteLine(GetContenido() + " ");
                Match(T.NUMERO);
            }
            else if (GetClasificacion() == T.IDENTIFICADOR)
            {
                Console.WriteLine(GetContenido() + " ");
                Match(T.IDENTIFICADOR);
            }
            else
            {
                Match("(");
                Expresion();
                Match(")");
            }
        }

        private void If()
        {
            Match("if");
            Match("(");
            Condicion();
            Match(")");
            if (GetClasificacion() == T.INICIO_BLOQUE)
            {
                BloqueDeInstrucciones();
            }
            else
            {
                Instruccion();
            }
            if (GetContenido() == "else")
            {
                Match("else");
                if (GetClasificacion() == T.INICIO_BLOQUE)
                {
                    BloqueDeInstrucciones();
                }
                else
                {
                    Instruccion();
                }
            }
        }

        private void For()
        {
            Match("for");
            Match("(");
            if (GetClasificacion() == T.TIPO_DATO)
            {
                Match(T.TIPO_DATO);
            }
            Match(T.IDENTIFICADOR);
            Match(T.ASIGNACION);
            Expresion();
            Match(T.FIN_SENTENCIA);
            Condicion();
            Match(T.FIN_SENTENCIA);
            ForExpresiones();
            Match(")");
            if (GetClasificacion() == T.INICIO_BLOQUE)
            {
                BloqueDeInstrucciones();
            }
            else
            {
                Instruccion();
            }
        }
        
        private void Condicion()
        {
            Expresion();
            Match(T.OPERADOR_RELACIONAL);
            Expresion();
            if (GetClasificacion() == T.OPERADOR_LOGICO)
            {
                Match(T.OPERADOR_LOGICO);
                Condicion();
            }
        }

        private void ForExpresiones()
        {
            if (GetClasificacion() == T.INCREMENTO_TERMINO)
            {
                Match(T.INCREMENTO_TERMINO);
                Match(T.IDENTIFICADOR);
            }
            else
            {
                Match(T.IDENTIFICADOR);
                if (GetClasificacion() == T.INCREMENTO_TERMINO)
                {
                    Match(T.INCREMENTO_TERMINO);
                }
                else
                {
                    Match(T.ASIGNACION);
                    Expresion();
                }
            }
            if (GetContenido() == ",")
            {
                Match(",");
                ForExpresiones();
            }
        }
    }
}
