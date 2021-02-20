using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDeClases
{
    public abstract class Interfaz<T>
    {
        protected abstract void Agregar(T jugador);
        protected abstract T BuscarId(Delegate delegates, T jugador);

        protected abstract void Eliminar(Delegate delegates, T jugador);
    }

    interface IInterfaz<T>
    {
        void Agregar();
        T BuscarId();

        void Eliminar();
        bool Vacia();
    }
}
