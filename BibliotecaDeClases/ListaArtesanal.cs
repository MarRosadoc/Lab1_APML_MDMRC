using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BibliotecaDeClases;

namespace BibliotecaDeClases
{
    public class ListaArtesanal<T> : Interfaz<T>, IEnumerable<T>
    {
        private Nodo<T> inicio { get; set; }
        private Nodo<T> fin { get; set; }
        public int contador = 0;


        public ListaArtesanal()
        {
            //inicio = null;
            //fin = null;
        }

        protected override void Agregar(T jugador)
        {
            Nodo<T> nuevo = new Nodo<T>();
            nuevo.DatosJugador = jugador;
            if (inicio == null)
            {
                inicio = nuevo;
                fin = nuevo;
            }
            else
            {
                fin.siguiente = nuevo;
                nuevo.anterior = fin;
                fin = nuevo;
            }
            contador++;
        }
        public void AddArtesanal(T jugador)
        {
            Agregar(jugador);
        }

        protected override T BuscarId(Delegate delegates, T DatosJugador)
        {
            Nodo<T> NodoBuscar = inicio;
            while (Convert.ToInt32(delegates.DynamicInvoke(NodoBuscar.DatosJugador, DatosJugador)) != 0)
            {
                NodoBuscar = NodoBuscar.siguiente;
            }
            return NodoBuscar.DatosJugador;
        }

        public T BuscarIdArtesanal(Delegate delegates, T DatosJugador)
        {
            return BuscarId(delegates, DatosJugador);
        }

        public void Editar(Delegate delegates, T DatosJugador)
        {
            Nodo<T> NodoJugador = new Nodo<T>();
            Nodo<T> NodoTemp = new Nodo<T>();
            NodoJugador.DatosJugador = DatosJugador;
            NodoTemp = inicio;
            while (NodoTemp != fin.siguiente)
            {
                if (Convert.ToInt32(delegates.DynamicInvoke(NodoTemp.DatosJugador, DatosJugador)) == 0)
                {
                    NodoTemp.DatosJugador = NodoJugador.DatosJugador;
                    break;
                }
                else
                {
                    NodoTemp = NodoTemp.Siguiente;
                }
            }
        }
        protected override void Eliminar(Delegate delegates, T DatosJugador)
        {
            Nodo<T> NodoJugador = new Nodo<T>();
            NodoJugador = inicio.siguiente;
            if (!Vacia())
            {
                if (Convert.ToInt32(delegates.DynamicInvoke(inicio.DatosJugador, DatosJugador)) == 0)
                {
                    if (contador == 1)
                    {
                        inicio = null;
                        fin = inicio;
                    }
                    else
                    {
                        inicio = inicio.siguiente;
                        inicio.anterior = null;
                    }
                }
                else if (Convert.ToInt32(delegates.DynamicInvoke(fin.DatosJugador, DatosJugador)) == 0)
                {
                    if (contador == 1)
                    {
                        fin = fin.siguiente;
                        inicio = fin;
                    }
                    else
                    {
                        fin = fin.anterior;
                        fin.siguiente = null;
                    }
                }
                else
                {
                    while (NodoJugador != fin)
                    {
                        if (Convert.ToInt32(delegates.DynamicInvoke(NodoJugador.DatosJugador, DatosJugador)) == 0)
                        {

                            NodoJugador.siguiente.anterior = NodoJugador.anterior;
                            NodoJugador.anterior.siguiente = NodoJugador.siguiente;
                            NodoJugador = fin;
                        }
                        else
                        {
                            NodoJugador = NodoJugador.Siguiente;
                        }
                    }
                }
            }
        }
        public void EliminarArtesanal(Delegate delegates, T DatosJugador)
        {
            Eliminar(delegates, DatosJugador);
        }
        public bool Vacia()
        {
            bool vacio;
            if (contador > 0)
            {
                vacio = false;
            }
            else
            {
                vacio = true;
            }
            return vacio;
        }
        public ListaArtesanal<T> FindAllArtesanal(Delegate delegates, T DatosJugador, ListaArtesanal<T> L)
        {
            //Buscar por posicion
            Nodo<T> NodoJugador = L.inicio;
            ListaArtesanal<T> ListaBusquedasA = new ListaArtesanal<T>();

            while (NodoJugador != L.fin.siguiente)
            {
                if (Convert.ToInt32(delegates.DynamicInvoke(NodoJugador.DatosJugador, DatosJugador)) == 0)
                {
                    //Si coincide se agrega a la lista de busqueda
                    ListaBusquedasA.AddArtesanal(NodoJugador.DatosJugador);
                    NodoJugador = NodoJugador.Siguiente;
                }
                else
                {
                    NodoJugador = NodoJugador.Siguiente;
                }
            }
            return ListaBusquedasA;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = inicio;
            while (node != null)
            {
                yield return node.DatosJugador;
                node = node.siguiente;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
