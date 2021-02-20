using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Lab1_1084120_1070720.Models;
using System.IO;
using System.Web;
using System.Text;
using BibliotecaDeClases;
using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Lab1_1084120_1070720.Controllers
{
    public class JugadoresController : Controller
    {
        public static ListaArtesanal<Jugadores> ListaArtesanalJugadores = new ListaArtesanal<Jugadores>();
        private static bool ListaCSharp;
        public static Stopwatch tiempos = new Stopwatch();

        public ActionResult ImportarCSV()
        {
            return View();
        }
        public ActionResult SeleccionListaAr()
        {
            System.IO.File.WriteAllText(@"CrearLog.txt", "  OPERACIONES CON TIEMPO \n");
            ListaCSharp = false;
            return View("JugadoresListas");
        }
        public ActionResult SeleccionListaC()
        {
            System.IO.File.WriteAllText(@"CrearLog.txt", "  OPERACIONES CON TIEMPO \n");
            ListaCSharp = true;
            return View("JugadoresListas");
        }

        //POST Importar Archivo
        [HttpPost]
        public IActionResult ImportarCSV(IFormFile ArchivoCSV)
        {
            tiempos.Restart();
            //Lee el archivo
            if (ArchivoCSV.FileName.Contains(".csv"))
            {

                try
                {
                    using (var reader = new StreamReader(ArchivoCSV.OpenReadStream()))
                    {
                        string archivolineas = reader.ReadToEnd().Remove(0, 72); //Elimina Encabezados
                        foreach (string linea in archivolineas.Split("\n"))
                        {
                            if (!string.IsNullOrEmpty(linea))
                            {
                                string[] lineajugador = linea.Split(",");
                                Jugadores Jugador = new Jugadores();
                                Jugador.Club = lineajugador[0];
                                Jugador.Apellido = lineajugador[1];
                                Jugador.Nombre = lineajugador[2];
                                Jugador.Posicion = lineajugador[3];
                                Jugador.Salario = Convert.ToDouble(lineajugador[4]);
                                if (ListaCSharp == true)
                                {
                                    //Agregar a la lista de C#
                                    Jugador.Id = Convert.ToInt32(Singleton.Instance.ListaJugadores.Count);
                                    Singleton.Instance.ListaJugadores.Add(Jugador);
                                }
                                else
                                {
                                    //Agregar a la lista artesanal}
                                    Jugador.Id = ListaArtesanalJugadores.contador;
                                    ListaArtesanalJugadores.AddArtesanal(Jugador);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    tiempos.Stop();
                    ViewBag.Message = "Operación Finalizada en " + tiempos + " milisegundos";
                    CrearLog("Importar Archivo de CSV, Tiempo empleado en milisegundos " + tiempos + "\n");
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception)
                {
                    //Mensaje de Error
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return View("ImportarCSV");
            }


        }



        // GET: JugadorController
        public ActionResult Index()
        {
            if (ListaCSharp == true)
            {
                //Mostrar a la lista de C#
                return View(Singleton.Instance.ListaJugadores);
            }
            if (ListaCSharp == false)
            {
                //Mostrar a la lista artesanal
                return View(ListaArtesanalJugadores);
            }
            return View();
        }

        // GET: JugadorController/Details/5
        public ActionResult DetallesJugador(int id)
        {
            var VistaJugador = Singleton.Instance.ListaJugadores.Find(x => x.Id == id);
            return View(VistaJugador);
        }

        // GET: JugadorController/Create
        public ActionResult CrearJugador()
        {
            return View();
        }

        // POST: JugadorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearJugador(IFormCollection collection)
        {
            try
            {
                tiempos.Restart();
                var nuevoJugador = new Models.Jugadores
                {

                    Nombre = collection["Nombre"],
                    Apellido = collection["Apellido"],
                    Posicion = collection["Posicion"],
                    Salario = Convert.ToDouble(collection["Salario"]),
                    Club = collection["Club"]
                };
                if (ListaCSharp == true)
                {
                    //Crear a la lista de C#
                    nuevoJugador.Id = Convert.ToInt32(Singleton.Instance.ListaJugadores.Count);
                    Singleton.Instance.ListaJugadores.Add(nuevoJugador);
                }
                else
                {
                    //Crear a la lista artesanal
                    nuevoJugador.Id = ListaArtesanalJugadores.contador;
                    ListaArtesanalJugadores.AddArtesanal(nuevoJugador);
                }
                tiempos.Stop();
                ViewBag.Message = "Operación Finalizada en " + tiempos + " milisegundos";
                CrearLog("Crear Jugador, Tiempo empleado en milisegundos " + tiempos + '\n');
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: JugadorController/Edit/5
        public ActionResult EditarJugador(int id)
        {

            Jugadores editarJugador = new Jugadores();
            editarJugador.Id = id;
            if (ListaCSharp == true)
            {
                //Busca en la lista de C#
                editarJugador = Singleton.Instance.ListaJugadores.Find(x => x.Id == id);
            }
            else
            {
                //Busca en la lista artesanal
                editarJugador = ListaArtesanalJugadores.BuscarIdArtesanal(editarJugador.SortById, editarJugador);
            }

            return View(editarJugador);
        }

        // POST: JugadorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarJugador(int id, IFormCollection collection)
        {
            try
            {
                tiempos.Restart();
                Jugadores editarJugador = new Jugadores();
                editarJugador.Id = id;
                editarJugador.Nombre = collection["Nombre"];
                editarJugador.Apellido = collection["Apellido"];
                editarJugador.Posicion = collection["Posicion"];
                editarJugador.Salario = Convert.ToDouble(collection["Salario"]);
                editarJugador.Club = collection["Club"];
                if (ListaCSharp == true)
                {
                    //Edita a la lista de C#
                    Singleton.Instance.ListaJugadores.Find(x => x.Id == id).Salario = Convert.ToDouble(collection["Salario"]);
                    Singleton.Instance.ListaJugadores.Find(x => x.Id == id).Club = collection["Club"];

                }
                else
                {
                    //Edita a la lista artesanal
                    ListaArtesanalJugadores.Editar(editarJugador.SortById, editarJugador);
                }
                tiempos.Stop();
                ViewBag.Message = "Operación Finalizada en " + tiempos + " milisegundos";
                CrearLog("Editar Jugador, Tiempo empleado en milisegundos " + tiempos + '\n');
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: JugadorController/Delete/5
        public ActionResult EliminarJugador(int id)
        {
            tiempos.Restart();
            Jugadores eliminarJugador = new Jugadores();
            eliminarJugador.Id = id;
            if (ListaCSharp == true)
            {
                //Elimina en la lista de C#
                eliminarJugador = Singleton.Instance.ListaJugadores.Find(x => x.Id == id);
            }
            else
            {
                //Elimina en la lista artesanal
                eliminarJugador = ListaArtesanalJugadores.BuscarIdArtesanal(eliminarJugador.SortById, eliminarJugador);
            }
            tiempos.Stop();
            ViewBag.Message = "Operación Finalizada en " + tiempos + " milisegundos";
            CrearLog("Eliminar Jugador, Tiempo empleado en milisegundos " + tiempos + '\n');
            return View(eliminarJugador);
            if (eliminarJugador == null)
            {
                return NotFound();
            }
        }

        // POST: JugadorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarJugador(int id, IFormCollection collection)
        {
            try
            {
                tiempos.Restart();
                Jugadores eliminarJugador = new Jugadores();
                eliminarJugador.Id = id;
                if (ListaCSharp == true)
                {
                    //Elimina de la lista de C#
                    Singleton.Instance.ListaJugadores.Remove(Singleton.Instance.ListaJugadores.Find(x => x.Id == id));
                }
                else
                {
                    //Elimina de la lista artesanal
                    ListaArtesanalJugadores.EliminarArtesanal(eliminarJugador.SortById, eliminarJugador);
                }
                tiempos.Stop();
                ViewBag.Message = "Operación Finalizada en " + tiempos + " milisegundos";
                CrearLog("Eliminar Jugador, Tiempo empleado en milisegundos " + tiempos + '\n');
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Busquedas(string Filtros, string TextoBusqueda)
        {
            tiempos.Restart();
            string filtro = Filtros;
            Jugadores buscarjugador = new Jugadores();
            List<Jugadores> ListaBusquedasC = new List<Jugadores>();
            ListaArtesanal<Jugadores> ListaBusquedasA = new ListaArtesanal<Jugadores>();

            if (filtro == "Nombre")
            {
                buscarjugador.Nombre = TextoBusqueda;
                if (ListaCSharp == true)
                {
                    //Lista de C#
                    ListaBusquedasC = Singleton.Instance.ListaJugadores.FindAll(x => x.Nombre == buscarjugador.Nombre);
                    return View(ListaBusquedasC);
                }
                else
                {
                    //Lista artesanal
                    return View(ListaBusquedasA.FindAllArtesanal(buscarjugador.SortByName, buscarjugador, ListaArtesanalJugadores));
                }
            }
            if (filtro == "Apellido")
            {
                buscarjugador.Apellido = TextoBusqueda;
                if (ListaCSharp == true)
                {
                    //Lista de C#
                    ListaBusquedasC = Singleton.Instance.ListaJugadores.FindAll(x => x.Apellido == buscarjugador.Apellido);
                    return View(ListaBusquedasC);
                }
                else
                {
                    //Lista artesanal
                    return View(ListaBusquedasA.FindAllArtesanal(buscarjugador.SortByApellido, buscarjugador, ListaArtesanalJugadores));
                }
            }
            if (filtro == "Posicion")
            {
                buscarjugador.Posicion = TextoBusqueda;
                if (ListaCSharp == true)
                {
                    //Lista de C#
                    buscarjugador.Posicion = TextoBusqueda;
                    ListaBusquedasC = Singleton.Instance.ListaJugadores.FindAll(x => x.Posicion == buscarjugador.Posicion);
                    return View(ListaBusquedasC);
                }
                else
                {
                    //Lista artesanal
                    return View(ListaBusquedasA.FindAllArtesanal(buscarjugador.SortByPosicion, buscarjugador, ListaArtesanalJugadores));
                }
            }
            if (filtro == "SalarioIgual")
            {
                buscarjugador.Salario = Convert.ToDouble(TextoBusqueda);
                if (ListaCSharp == true)
                {
                    //Lista de C#
                    ListaBusquedasC = Singleton.Instance.ListaJugadores.FindAll(x => x.Salario == buscarjugador.Salario);
                    return View(ListaBusquedasC);
                }
                else
                {
                    //Lista artesanal
                    return View(ListaBusquedasA.FindAllArtesanal(buscarjugador.SortBySalarioIgual, buscarjugador, ListaArtesanalJugadores));
                }
            }
            if (filtro == "SalarioMayor")
            {
                buscarjugador.Salario = Convert.ToDouble(TextoBusqueda);
                if (ListaCSharp == true)
                {
                    //Lista de C#
                    ListaBusquedasC = Singleton.Instance.ListaJugadores.FindAll(x => x.Salario > buscarjugador.Salario);
                    return View(ListaBusquedasC);
                }
                else
                {
                    //Lista artesanal
                    return View(ListaBusquedasA.FindAllArtesanal(buscarjugador.SortBySalarioMayor, buscarjugador, ListaArtesanalJugadores));
                }
            }
            if (filtro == "SalarioMenor")
            {
                buscarjugador.Salario = Convert.ToDouble(TextoBusqueda);
                if (ListaCSharp == true)
                {
                    //Lista de C#
                    ListaBusquedasC = Singleton.Instance.ListaJugadores.FindAll(x => x.Salario < buscarjugador.Salario);
                    return View(ListaBusquedasC);
                }
                else
                {
                    //Lista artesanal
                    return View(ListaBusquedasA.FindAllArtesanal(buscarjugador.SortBySalarioMenor, buscarjugador, ListaArtesanalJugadores));
                }
            }
            if (filtro == "Club")
            {
                buscarjugador.Club = TextoBusqueda;
                if (ListaCSharp == true)
                {
                    //Lista de C#
                    buscarjugador.Club = TextoBusqueda;
                    ListaBusquedasC = Singleton.Instance.ListaJugadores.FindAll(x => x.Club == buscarjugador.Club);
                    return View(ListaBusquedasC);
                }
                else
                {
                    //Lista artesanal
                    return View(ListaBusquedasA.FindAllArtesanal(buscarjugador.SortByClub, buscarjugador, ListaArtesanalJugadores));
                }
            }
            tiempos.Stop();
            ViewBag.Message = "Operación Finalizada en " + tiempos + " milisegundos";
            CrearLog("Buscar a Jugador, Tiempo empleado en milisegundos " + tiempos + '\n');
            if (ListaCSharp == true)
            {
                return View(Singleton.Instance.ListaJugadores);
            }
            else
            {
                return View(ListaArtesanalJugadores);
            }
        }

        private string ruta = "";
        public void CrearLog(string TextoLog)
        {
            //CrearDirectorio();
            TextoLog = "      " +'\n' + TextoLog + "Tiempo de ejecución de operación en milisegundos: " + tiempos.ElapsedMilliseconds;
            string nombrearchivo = "log_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".txt";
            System.IO.File.AppendAllText(nombrearchivo, TextoLog);
        }
        //private void CrearDirectorio()
        //{
        //    try
        //    {
        //        if (!Directory.Exists(ruta))
        //            Directory.CreateDirectory(ruta);
        //    }
        //    catch (DirectoryNotFoundException ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
