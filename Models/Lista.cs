using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1_1084120_1070720.Models
{
    public sealed class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public List<Jugadores> ListaJugadores;

        private Singleton()
        {
            ListaJugadores = new List<Jugadores>();
        }
        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
