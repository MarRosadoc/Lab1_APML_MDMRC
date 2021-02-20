using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Lab1_1084120_1070720.Models
{
    public class Jugadores : IComparable
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string Posicion { get; set; }
        [Required]
        public double Salario { get; set; }
        [Required]
        public string Club { get; set; }

        public int CompareTo(object obj)
        {
            var comparer = ((Jugadores)obj).Id;
            return comparer < Id ? 1 : comparer == Id ? 0 : -1;
        }

        public Comparison<Jugadores> SortById = delegate (Jugadores j1, Jugadores j2)
        {
            return j1.Id.CompareTo(j2.Id);
        };
        public Comparison<Jugadores> SortByName = delegate (Jugadores j1, Jugadores j2)
        {
            return j1.Nombre.CompareTo(j2.Nombre);
        };
        public Comparison<Jugadores> SortByApellido = delegate (Jugadores j1, Jugadores j2)
        {
            return j1.Apellido.CompareTo(j2.Apellido);
        };
        public Comparison<Jugadores> SortByPosicion = delegate (Jugadores j1, Jugadores j2)
        {
            return j1.Posicion.CompareTo(j2.Posicion);
        };
        public Comparison<Jugadores> SortBySalarioIgual = delegate (Jugadores j1, Jugadores j2)
        {
            return j1.Salario.CompareTo(j2.Salario);
        };
        public Comparison<Jugadores> SortBySalarioMayor = delegate (Jugadores j1, Jugadores j2)
        {
            int salariobase = j1.Salario.CompareTo(j2.Salario);
            //Es igual o menor
            if (salariobase == 0 || salariobase == -1)
            {
                return -1;
            }
            //Es mayor
            else
            {
                return 0;
            }
        };
        public Comparison<Jugadores> SortBySalarioMenor = delegate (Jugadores j1, Jugadores j2)
        {
            int salariobase = j1.Salario.CompareTo(j2.Salario);
            //Es igual o mayor
            if (salariobase == 0 || salariobase == 1)
            {
                return -1;
            }
            //Es menor
            else
            {
                return 0;
            }
        };
        public Comparison<Jugadores> SortByClub = delegate (Jugadores j1, Jugadores j2)
        {
            return j1.Club.CompareTo(j2.Club);
        };


    }


}
