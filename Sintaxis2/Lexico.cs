using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Sintaxis2
{
    class Lexico : Token, IDisposable
    {
        private bool disposed = false;

        //Implement IDisposable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects)
                    Console.WriteLine("Fin de análisis léxico.");
                    CloseLog();
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                disposed = true;
            }
        }

        public StreamReader archivo;//cambiamos private a public para probar algo
        protected StreamWriter log;
        const int o = -4;
        const int c = -3;
        const int e = -2;
        const int f = -1;
        int cols = 0;
        int rows = 1;

        public int GetCols()
        {
            return cols;
        }

        public int GetRows()
        {
            return rows;
        }

        int[,] TRAND = {                //ws  l  d  .  e  +  -  la ;  {  }  *  /  %  =  &  |  !  <  >  "  '  # \n end
                            /*Estado 0 */{ 0, 1, 2, 8, 1,12,25, 8, 8, 8, 8,13,29,13,14,15,17,19,20,21,32,34,37, 0, f },
                            /*Estado 1 */{ f, 1, 1, f, 1, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 2 */{ f, f, 2, 3, 5, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 3 */{ e, e, 4, e, e, e, e, e, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 4 */{ f, f, 4, f, 5, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 5 */{ e, e, 7, e, e, 6, 6, e, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 6 */{ e, e, 7, e, e, e, e, e, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 7 */{ f, f, 7, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 8 */{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 9 */{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 10*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 11*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 12*/{ f, f, f, f, f,26, f, f, f, f, f, f, f, f,26, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 13*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f,31, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 14*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f,22, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 15*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f,16, f, f, f, f, f, f, f, f, f },
                            /*Estado 16*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 17*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f,18, f, f, f, f, f, f, f, f },
                            /*Estado 18*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 19*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f,22, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 20*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f,22, f, f, f,23, f, f, f, f, f, f },
                            /*Estado 21*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f,22, f, f, f, f,23, f, f, f, f, f },
                            /*Estado 22*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 23*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 24*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 25*/{ f, f, f, f, f, f,26, f, f, f, f, f, f, f,26, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 26*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 27*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 28*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 29*/{ f, f, f, f, f, f, f, f, f, f, f,41,40, f,31, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 30*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 31*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 32*/{32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,32,33,32,32,32, e },
                            /*Estado 33*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 34*/{35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35,35, e,35,35, e },
                            /*Estado 35*/{ e, e, e, e, e, e, e, e, e, e, e, e, e, e, e, e, e, e, e, e, e,36, e, e, e },
                            /*Estado 36*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 37*/{ f, f,38, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 38*/{ c, c,38, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c, c },
                            /*Estado 39*/{ f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f },
                            /*Estado 40*/{40,40,40,40,40,40,40,40,40,40,40,40,40,40,40,40,40,40,40,40,40,40,40, 0, 0 },
                            /*Estado 41*/{41,41,41,41,41,41,41,41,41,41,41,42,41,41,41,41,41,41,41,41,41,41,41,41, 0 },
                            /*Estado 42*/{41,41,41,41,41,41,41,41,41,41,41,42, 0,41,41,41,41,41,41,41,41,41,41,41, 0 },
                      };

        public Lexico()
        {
            archivo = new StreamReader(@"C:\archivos\Prueba.cs");
            log = new StreamWriter(@"C:\archivos\prueba.log");
            log.WriteLine("Programador: Armando Serna");
        }

        public Lexico(string nombre)
        {
            archivo = new StreamReader(nombre);
            log = new StreamWriter(@"C:\archivos\prueba.log");
            log.WriteLine("Programador: Armando Serna");
        }

        ~Lexico()
        {
        }

        private int Columna(char transicion)
        {
            if (archivo.EndOfStream)
            {
                return 24;
            }
            else if (transicion == '\n')
            {
                return 23;
            }
            else if (char.IsWhiteSpace(transicion))
            {
                return 0;
            }
            else if (char.ToLower(transicion) == 'e')
            {
                return 4;
            }
            else if (char.IsLetter(transicion))
            {
                return 1;
            }
            else if (char.IsDigit(transicion))
            {
                return 2;
            }
            else if (transicion == '.')
            {
                return 3;
            }
            else if (transicion == '+')
            {
                return 5;
            }
            else if (transicion == '-')
            {
                return 6;
            }
            else if (transicion == ';')
            {
                return 8;
            }
            else if (transicion == '{')
            {
                return 9;
            }
            else if (transicion == '}')
            {
                return 10;
            }
            else if (transicion == '*')
            {
                return 11;
            }
            else if (transicion == '/')
            {
                return 12;
            }
            else if (transicion == '%')
            {
                return 13;
            }
            else if (transicion == '=')
            {
                return 14;
            }
            else if (transicion == '&')
            {
                return 15;
            }
            else if (transicion == '|')
            {
                return 16;
            }
            else if (transicion == '!')
            {
                return 17;
            }
            else if (transicion == '<')
            {
                return 18;
            }
            else if (transicion == '>')
            {
                return 19;
            }
            else if (transicion == '"')
            {
                return 20;
            }
            else if (transicion == '\'')
            {
                return 21;
            }
            else if (transicion == '#')
            {
                return 22;
            }
            else
            {
                return 7;
            }
        }

        private int Automata(int estado, char transicion)
        {
            int columna = Columna(transicion);
            switch (TRAND[estado, columna])
            {
                case 1:
                    SetClasificacion(T.IDENTIFICADOR);
                    break;
                case 2:
                    SetClasificacion(T.NUMERO);
                    break;
                case 8:
                    if (transicion == ';')
                    {
                        SetClasificacion(T.FIN_SENTENCIA);
                    }
                    else if (transicion == '{')
                    {
                        SetClasificacion(T.INICIO_BLOQUE);
                    }
                    else if (transicion == '}')
                    {
                        SetClasificacion(T.FIN_BLOQUE);
                    }
                    else
                    {
                        SetClasificacion(T.CARACTER);
                    }
                    break;
                case 12:
                case 25:
                    SetClasificacion(T.OPERADOR_TERMINO);
                    break;
                case 13:
                case 29:
                    SetClasificacion(T.OPERADOR_FACTOR);
                    break;
                case 14:
                    SetClasificacion(T.ASIGNACION);
                    break;
                case 15:
                case 17:
                    SetClasificacion(T.CARACTER);
                    break;
                case 16:
                case 18:
                case 19:
                    SetClasificacion(T.OPERADOR_LOGICO);
                    break;
                case 20:
                case 21:
                case 22:
                    SetClasificacion(T.OPERADOR_RELACIONAL);
                    break;
                case 23:
                    SetClasificacion(T.OPERADOR_FLUJO);
                    break;
                case 26:
                    SetClasificacion(T.INCREMENTO_TERMINO);
                    break;
                case 31:
                    SetClasificacion(T.INCREMENTO_FACTOR);
                    break;
                case 34:
                    SetClasificacion(T.CARACTER);
                    break;
                case 32:
                    SetClasificacion(T.CADENA);
                    break;
                case 37:
                    SetClasificacion(T.CARACTER);
                    break;
                case e:
                case o:
                    try
                    {
                        ThrowException(estado);
                    }
                    catch (Exception)
                    {
                    }
                    break;
                default:
                    break;
            }
            return TRAND[estado, columna];
        }

        private void ThrowException(int estado)
        {
            if (estado == 35)
            {
                throw new CharException(log);
            }
            else if (estado == 34)
            {
                throw new CharException(log);
            }
            else if (estado == 3)
            {
                throw new DecimalNumberException(log);
            }
            else if (estado == 5)
            {
                throw new NotationException(log);
            }
            else if (estado == 6)
            {
                throw new NotationException(log);
            }
            else if (estado == 32)
            {
                throw new EndOfStringException(log);
            }
            else if (estado == 0)
            {
                throw new CommentException(log);
            }
        }

        public void NextToken()
        {
            char transicion;
            string buffer = "";
            int estado = 0;
            while (estado >= 0)
            {
                transicion = (char)archivo.Peek();
                if (transicion == '\n')
                {
                    rows++;
                    cols = 0;
                }
                else if (transicion == '\t')
                {
                    cols += 8;
                }
                estado = Automata(estado, transicion);
                if (estado >= 0)
                {
                    cols++;
                    if (estado > 0)
                        buffer += transicion;
                    else
                    {
                        buffer = "";
                    }
                    archivo.Read();
                }
                else if (estado == c)
                {
                    try
                    {
                        if (int.Parse(buffer.Substring(1)) > 255 || int.Parse(buffer.Substring(1)) < 1)
                            throw new UTFException(log);
                    }
                    catch (UTFException)
                    {
                        log.WriteLine(": Línea:" + rows + ", Columna: " + cols + "\n");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(": Línea:" + rows + ", Columna: " + cols + "\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        estado = e;
                    }
                }
            }
            if (GetClasificacion() == T.IDENTIFICADOR)
            {
                switch (buffer)
                {
                    case "char":
                    case "int":
                    case "float":
                    case "string":
                        SetClasificacion(T.TIPO_DATO);
                        break;
                    case "private":
                    case "protected":
                    case "public":
                        SetClasificacion(T.TIPO_ZONA);
                        break;
                    case "const":
                        SetClasificacion(T.CONSTANTE);
                        break;
                }
            }
            SetContenido(buffer);
        }

        public void CloseLog()
        {
            log.Close();
        }
    }

    class DecimalNumberException : Exception
    {
        public DecimalNumberException(StreamWriter log)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error Lexico: Se espera un dígito");
            Console.ForegroundColor = ConsoleColor.White;
            log.WriteLine("Error Lexico: Se espera un dígito");
        }
    }

    class NotationException : Exception
    {
        public NotationException(StreamWriter log)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error Lexico: Se espera un dígito despues de 'e'");
            Console.ForegroundColor = ConsoleColor.White;
            log.WriteLine("Error Lexico: Se espera un dígito despues de 'e'");
        }
    }

    class EndOfStringException : Exception
    {
        public EndOfStringException(StreamWriter log)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error Lexico: Se espera '\"'");
            Console.ForegroundColor = ConsoleColor.White;
            log.WriteLine("Error Lexico: Se espera '\"'");
        }
    }
    class CharException : Exception
    {
        public CharException(StreamWriter log)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error Lexico: Se esperaba un caracter");
            Console.ForegroundColor = ConsoleColor.White;
            log.WriteLine("Error Lexico: Se esperaba un caracter");
        }
        public CharException(StreamWriter log, String msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
            log.WriteLine(msg);
        }
    }
    class UTFException : Exception
    {
        public UTFException(StreamWriter log)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error Lexico: Se espera un digito valido");
            Console.ForegroundColor = ConsoleColor.White;
            log.WriteLine("Error Lexico: Se esperaba un digito valido");
        }
    }


    class CommentException : Exception
    {
        public CommentException(StreamWriter log)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Se esperaba un fin de comentario");
            Console.ForegroundColor = ConsoleColor.White;
            log.WriteLine("Se esperaba un fin de comentario");
        }
    }

}
