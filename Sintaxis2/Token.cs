using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintaxis2
{
    public enum T
    {
        IDENTIFICADOR,
        NUMERO,
        CARACTER,
        FIN_SENTENCIA,
        INICIO_BLOQUE,
        FIN_BLOQUE,
        OPERADOR_TERMINO, //Signo más y menos
        OPERADOR_FACTOR,
        ASIGNACION,
        OPERADOR_LOGICO,
        OPERADOR_RELACIONAL,
        INCREMENTO_TERMINO,
        INCREMENTO_FACTOR,
        OPERADOR_FLUJO,
        CADENA,
        TIPO_DATO, //char, int, float
        TIPO_ZONA, //private, protected, public
        CONSTANTE,
        FIN
    }

    class Token
    {
        private T clasificacion;
        private string contenido;

        public T GetClasificacion()
        {
            return clasificacion;
        }
        public string GetContenido()
        {
            return contenido;
        }
        public void SetClasificacion(T clasificacion)
        {
            this.clasificacion = clasificacion;
        }
        public void SetContenido(string contenido)
        {
            this.contenido = contenido;
        }
        public string CtoStr(T clasificacion)
        {
            switch (clasificacion)
            {
                case T.IDENTIFICADOR:
                    return "identificador";
                case T.NUMERO:
                    return "numero";
                case T.CARACTER:
                    return "caracter";
                case T.FIN_SENTENCIA:
                    return "fin de sentencia";
                case T.INICIO_BLOQUE:
                    return "inicio de bloque";
                case T.FIN_BLOQUE:
                    return "fin de bloque";
                case T.OPERADOR_TERMINO:
                    return "Operador de termino";
                case T.OPERADOR_FACTOR:
                    return "Operador de factor";
                case T.ASIGNACION:
                    return "Asignación";
                case T.OPERADOR_LOGICO:
                    return "Operador lógico";
                case T.OPERADOR_RELACIONAL:
                    return "Operador relacional";
                case T.INCREMENTO_TERMINO:
                    return "Incremento Termino";
                case T.INCREMENTO_FACTOR:
                    return "Incremento factor";
                case T.OPERADOR_FLUJO:
                    return "Operador de flujo";
                case T.CADENA:
                    return "Cadena";
                case T.TIPO_DATO:
                    return "Tipo de dato";
                case T.TIPO_ZONA:
                    return "Zona de accesibilidad";
                case T.CONSTANTE:
                    return "Constante";
                default:
                    return "No se reconoce";
            }
        }
    }
}

