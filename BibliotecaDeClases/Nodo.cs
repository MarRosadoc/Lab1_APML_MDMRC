using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDeClases
{
    class Nodo<T>
    {

        public Nodo<T> siguiente;
        public Nodo<T> anterior;
        public T DatosJugador { get; set; }

        public Nodo<T> Siguiente
        {
            get { return siguiente; }
            set { siguiente = value; }
        }

        public Nodo<T> Anterior
        {
            get { return anterior; }
            set { anterior = value; }
        }

    }

}