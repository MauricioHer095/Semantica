using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


/*
1. Colocar la linea en los errores lexicos y sintaxicos
2. Cambiar clase token por atributos publicos usando get y set
3. Cambiar los contructores de la calse lexico usando parametros por default

*/

namespace Semantica
{

    public class Lenguaje : Sintaxis
    {

        public Lenguaje()
        {


        }

        public Lenguaje(string nombre) : base(nombre)
        {


        }

        // Program -> Librerias? Variables? Main
        public void Programa()
        {
            if (getContenido() == "using")
            {
                Librerias();
            }
            Main();

            //:D
        }

        //Librerias -> using ListaLibrerias; Librerias?
        private void Librerias()
        {
            match("using");
            ListaLibrerias();
            match(";");
            if (getContenido() == "using")
            {
                Librerias();
            }
        }

        //ListaLibrerias -> identificador (.ListaLibrerias)?
        private void ListaLibrerias()
        {
            match(Tipos.Identificador);
            if (getContenido() == ".")
            {
                match(".");
                ListaLibrerias();
            }
        }

        //--------------------------------------------------------------------------------------------

        //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            match(Tipos.TipoDato);
            listaIdentificadores();
            match(";");
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
        }

        //ListaIdentificadores -> identificador (,ListaIdentificadores)?
        private void listaIdentificadores()
        {
            match(Tipos.Identificador);
            if (getContenido() == ",")
            {
                match(",");
                listaIdentificadores();
            }
        }

        //BloqueInstrucciones -> { listaIntrucciones? }
        private void bloqueInstrucciones()
        {
            match("{");
            if (getContenido() != "}")
            {
                listaInstrucciones();
            }
            match("}");
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void listaInstrucciones()
        {
            Instruccion();
            if (getContenido() != "}")
            {
                listaInstrucciones();
            }
        }

        //Instruccion -> Console | If | While | do | For | Asignacion
        private void Instruccion()
        {
            if (getContenido() == "Console")
            {
                Console();
            }
            else if (getContenido() == "if")
            {
                If();
            }
            else if (getContenido() == "while")
            {
                While();
            }
            else if (getContenido() == "do")
            {
                Do();
            }
            else if (getContenido() == "for")
            {
                For();
            }
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
            else
            {
                Asignacion();
            }
        }

        // Asignacion -> Identificador = Expresion;
        private void Asignacion()
        {
            match(Tipos.Identificador);
            match("=");
            Expresion();
            match(";");
        }

        //If -> if (Condicion) bloqueInstrucciones | instruccion (else bloqueInstrucciones | instruccion)?
        private void If()
        {
            match("if");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
            {
                bloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
            if (getContenido() == "else")
            {
                match("else");
                if (getContenido() == "{")
                {
                    bloqueInstrucciones();
                }
                else
                {
                    Instruccion();
                }
            }
        }

        //Condicion -> Expresion operadorRelacional Expresion
        private void Condicion()
        {
            Expresion();
            match(Tipos.OpRelacional);
            Expresion();
        }

        //While -> while(Condicion) bloqueInstrucciones | instruccion
        private void While()
        {
            match("while");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
            {
                bloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
        }

        //Do -> do bloqueInstrucciones | intruccion while(Condicion);
        private void Do()
        {
            match("do");
            if (getContenido() == "{")
            {
                bloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
            match("while");
            match("(");
            Condicion();
            match(")");
            match(";");
        }

        //For -> for(Asignacion Condicion; Incremento) BloqueInstrucciones | Intruccion
        private void For()
        {
            match("for");
            match("(");
            Asignacion();
            Condicion();
            match(";");
            Incremento();
            match(")");
            if (getContenido() == "{")
            {
                bloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }

        }

        //Incremento -> Identificador ++ | --
        private void Incremento()
        {
            match(Tipos.Identificador);
            if (getContenido() == "++")
            {
                match("++");
            }
            else
            {
                match("--");
            }
        }

        //Console -> Console.(WriteLine|Write) (cadena?); | Console.(Read | ReadLine) ();
        private void Console()
        {
            match("Console");
            match(".");
            if (getContenido() == "WriteLine" || getContenido() == "Write")
            {
                match(getContenido());
                match("(");
                if (getClasificacion() == Tipos.Cadena)
                {
                    match(Tipos.Cadena);
                }
                match(")");
                match(";");
            }
            else
            {
                if (getContenido() == "ReadLine")
                {
                    match("ReadLine");
                }
                else
                {
                    match("Read");
                }
                match("(");
                match(")");

            }
            match(";");
        }

        //Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }

        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }

        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (getClasificacion() == Tipos.OpTermino)
            {
                match(Tipos.OpTermino);
                Termino();
            }
        }

        //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (getClasificacion() == Tipos.Numero)
            {
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                match(Tipos.Identificador);
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }

        //PorFactor -> (OperadorFactor Factor)?
        private void PorFactor()
        {
            if (getClasificacion() == Tipos.OpFactor)
            {
                match(Tipos.OpFactor);
                Factor();
            }
        }

        //Main      -> static void Main(string[] args) BloqueInstrucciones
        private void Main()
        {
            match("static");
            match("void");
            match("Main");
            match("(");
            match("string");
            match("[");
            match("]");
            match("args");
            match(")");
            bloqueInstrucciones();
        }

    }

}