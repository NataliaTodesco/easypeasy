using System.Xml;
using System.Data;
using System.Xml.Schema;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System;
using Microsoft.AspNetCore.Mvc;
using EasyPeasy.Models;
using EasyPeasy.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using api.Resultados;
using EasyPeasy.Comandos;


namespace api.Controllers
{
    [ApiController]
    [EnableCors("postgres")]
    public class HojaDeRutaController : ControllerBase
    {
        private readonly EasyPeasyDBContext _db = new EasyPeasyDBContext();

        [HttpGet]
        [Route("HojaDeRuta/ObtenerHojasDeRuta")]

        public ActionResult<ResultadoApi> Get()
        {
            var Resultado = new ResultadoApi();
            try
            {
                Resultado.Ok = true;
                Rutas Hr = new Rutas();
                Hr.HojaRutas = (from h in _db.HojaRuta
                                 select new DTOHojaRuta
                                 {
                                     Id = h.IdHojaRuta,
                                     Fecha = h.Fecha,
                                     IdVehiculo = h.IdVehiculo,
                                     Vehiculo = new HRVehiculo{
                                                IdVehiculo = h.IdVehiculoNavigation.IdVehiculo,
                                                Patente = h.IdVehiculoNavigation.Patente,
                                                Modelo = h.IdVehiculoNavigation.Modelo
                                                },
                                     IdTransportista = h.IdTransportista,
                                     Transportista =  new HRTransportista{
                                                        IdTransportista = h.IdTransportistaNavigation.IdTransportista, 
                                                        Legajo = h.IdTransportistaNavigation.Legajo, 
                                                        Nombre= h.IdTransportistaNavigation.Nombre, 
                                                    },
                                     IdZona = h.Remitos.FirstOrDefault().IdClienteNavigation.IdBarrioNavigation.IdZonaNavigation.IdZona,
                                     Remitos = (
                                         from r in _db.Remitos
                                         where r.IdHojaRuta == h.IdHojaRuta
                                         select new HRemito
                                         {
                                             IdRemito = r.IdRemito,
                                             FechaCompra = r.FechaCompra,
                                             HoraEntregaPreferido = r.HoraEntregaPreferido,
                                             Estado = r.IdEstado,
                                             Cliente = (
                                                 from c in _db.Clientes
                                                 where c.IdCliente == r.IdCliente
                                                 select new HCliente{
                                                    IdCliente = c.IdCliente,
                                                    Nombre = c.Nombre,
                                                    Direccion = c.Direccion,
                                                    Telefono = c.Telefono
                                                 }
                                             ).FirstOrDefault()
                                         }
                                       ).ToList(),

                                    //  un listado de las direcciones de cada hoja de ruta para haccer mas facil el acceso al mapa
                                     Direcciones = (
                                       from a in _db.Remitos
                                       join b in _db.HojaRuta on a.IdHojaRuta equals b.IdHojaRuta
                                       join c in _db.Clientes on a.IdCliente equals c.IdCliente
                                       where a.IdHojaRuta == h.IdHojaRuta
                                       select c.Direccion).ToList()
                                 }).ToList();
                Resultado.Return = Hr;
                return Resultado;
            }
            catch (Exception ex)
            {
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }

        //update/create get
        [HttpGet]
        [Route("HojaDeRuta/Upsert")]
        public ActionResult<ResultadoApi> Upsert(int? Id)
        {

            var Resultado = new ResultadoApi();
            try
                {
                Resultado.Ok = true;
            //     Resultado.Return = _db.HojaRuta
            //                          .Include(x => x.Remitos)
            //                          .FirstOrDefault(x => x.IdHojaRuta == Id);
            DTOHojaRuta Hr = new DTOHojaRuta();
                        Hr = (from h in _db.HojaRuta
                                where h.IdHojaRuta == Id
                                 select new DTOHojaRuta
                                 {
                                     Id = h.IdHojaRuta,
                                     Fecha = h.Fecha,
                                     IdVehiculo = h.IdVehiculo,
                                     Vehiculo = new HRVehiculo{
                                                IdVehiculo = h.IdVehiculoNavigation.IdVehiculo,
                                                Patente = h.IdVehiculoNavigation.Patente,
                                                Modelo = h.IdVehiculoNavigation.Modelo
                                                },
                                     IdTransportista = h.IdTransportista,
                                     Transportista =  new HRTransportista{
                                                        IdTransportista = h.IdTransportistaNavigation.IdTransportista, 
                                                        Legajo = h.IdTransportistaNavigation.Legajo, 
                                                        Nombre= h.IdTransportistaNavigation.Nombre, 
                                                    },
                                     IdZona = h.Remitos.FirstOrDefault().IdClienteNavigation.IdBarrioNavigation.IdZonaNavigation.IdZona,
                                     Remitos = (
                                         from r in _db.Remitos
                                         where r.IdHojaRuta == h.IdHojaRuta
                                         select new HRemito
                                         {
                                             IdRemito = r.IdRemito,
                                             FechaCompra = r.FechaCompra,
                                             HoraEntregaPreferido = r.HoraEntregaPreferido,
                                             Estado = r.IdEstado,
                                             Cliente = (
                                                 from c in _db.Clientes
                                                 where c.IdCliente == r.IdCliente
                                                 select new HCliente{
                                                    IdCliente = c.IdCliente,
                                                    Nombre = c.Nombre,
                                                    Direccion = c.Direccion,
                                                    Telefono = c.Telefono
                                                 }
                                             ).FirstOrDefault()
                                         }
                                       ).ToList(),

                                    //  un listado de las direcciones de cada hoja de ruta para haccer mas facil el acceso al mapa
                                     Direcciones = (
                                       from a in _db.Remitos
                                       join b in _db.HojaRuta on a.IdHojaRuta equals b.IdHojaRuta
                                       join c in _db.Clientes on a.IdCliente equals c.IdCliente
                                       where a.IdHojaRuta == h.IdHojaRuta
                                       select c.Direccion).ToList()
                                 }).FirstOrDefault();
                Resultado.Return = Hr;
                return Resultado;

            }
            catch (Exception ex)
            {
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }



        }

        [HttpPost]
        [Route("HojaDeRuta/RegistrarHojaDeRuta")]
        public ActionResult<ResultadoApi> AltaHojaRuta([FromBody] ComandoHojaRuta comando)
        {
            var resultado = new ResultadoApi();

            var hojaRuta = new HojaRuta();
            hojaRuta.Fecha = comando.Fecha;
            hojaRuta.IdVehiculo = comando.IdVehiculo;
            hojaRuta.IdTransportista = comando.IdTransportista;
            //busco los remitos pendientes para la zona seleccionada y registro la hoja de ruta
            hojaRuta.Remitos = _db.Remitos
                            .Where(x => x.IdClienteNavigation.IdBarrioNavigation.IdZona == comando.idZona
                            && x.IdEstado == 1)
                            .Take(3)
                            .ToList();
            //agrego hoja de ruta
            _db.HojaRuta.Add(hojaRuta);
            //cambio el estado de los remitos de "1"(pendiente) a "2"(en proceso)
            foreach (var item in hojaRuta.Remitos)
            {
                item.IdEstado = 2;
            }
            //cambio la disponibilidad del transportista
            var transportista=_db.Transportistas.Where(x=>x.IdTransportista==hojaRuta.IdTransportista).FirstOrDefault();
            transportista.idDisponibilidad=2;
            //guardo cambios
            _db.SaveChanges();

            resultado.Ok = true;
            resultado.Return = _db.HojaRuta.ToList();

            return resultado;
        }

        //get hoja de ruta segun legajo de transportista
        [HttpGet]
        [Route("HojaDeRuta/ObtenerHojaRutaTransportista")]
        public ActionResult<ResultadoApi> Get(int legajo)
        {

            var Resultado = new ResultadoApi();
            try
            {
                Rutas hr = new Rutas();
                Resultado.Ok = true;
                // Resultado.Return = _db.HojaRuta
                //                    .Include(x => x.Remitos)
                //                    .ThenInclude(x => x.IdClienteNavigation)
                //                    .ThenInclude(x => x.IdBarrioNavigation)
                //                    .ThenInclude(x => x.IdZonaNavigation) vade retro referencia circular
                //                    .Include(x => x.IdTransportistaNavigation)
                //                    .Include(x => x.IdVehiculoNavigation)
                //                    .FirstOrDefault(x => x.IdTransportistaNavigation.Legajo == legajo);
                hr.HojaRutas =(from h in _db.HojaRuta
                                
                                where h.IdTransportista == legajo

                                 select new DTOHojaRuta
                                 {
                                     Id = h.IdHojaRuta,
                                     Fecha = h.Fecha,
                                     IdVehiculo = h.IdVehiculo,
                                     IdTransportista = h.IdTransportista,

                                     Remitos = (
                                         from r in _db.Remitos
                                         where r.IdHojaRuta == h.IdHojaRuta
                                         select new HRemito
                                         {
                                             IdRemito = r.IdRemito,
                                             FechaCompra = r.FechaCompra,
                                             HoraEntregaPreferido = r.HoraEntregaPreferido,
                                             Estado = r.IdEstado,
                                             Cliente = (
                                                 from c in _db.Clientes
                                                 where c.IdCliente == r.IdCliente
                                                 select new HCliente{
                                                    IdCliente = c.IdCliente,
                                                    Nombre = c.Nombre,
                                                    Direccion = c.Direccion,
                                                    Telefono = c.Telefono
                                                 }
                                             ).FirstOrDefault()
                                         }
                                       ).ToList(),

                                    //  un listado de las direcciones de cada hoja de ruta para haccer mas facil el acceso al mapa
                                     Direcciones = (
                                       from a in _db.Remitos
                                       join b in _db.HojaRuta on a.IdHojaRuta equals b.IdHojaRuta
                                       join c in _db.Clientes on a.IdCliente equals c.IdCliente
                                       where a.IdHojaRuta == h.IdHojaRuta
                                       select c.Direccion).ToList()
                                 }).ToList();
                Resultado.Return = hr;
                return Resultado;

            }
            catch (Exception ex)
            {
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }


        }

        //actualizar hoja de ruta (solo updetear fecha/transportista/vehiculo)
        [HttpPut]
        [Route("HojaDeRuta/Actualizar")]
        public ActionResult<ResultadoApi> Update([FromBody] ComandoHojaRuta comando)
        {
            var resultado = new ResultadoApi();


            var hojaDeRuta = _db.HojaRuta.FirstOrDefault(x => x.IdHojaRuta == comando.IdHojaRuta);
            if (hojaDeRuta != null)
            {
                hojaDeRuta.Fecha = comando.Fecha;
                hojaDeRuta.IdTransportista = comando.IdTransportista;
                hojaDeRuta.IdVehiculo = comando.IdVehiculo;
                _db.HojaRuta.Update(hojaDeRuta);
                _db.SaveChanges();
            }

            resultado.Ok = true;
            resultado.Return = _db.HojaRuta.ToList();

            return resultado;
        }

        [HttpDelete]
        [Route("/HojaDeRuta/Eliminar")]
        public ActionResult<ResultadoApi> Delete(int id)
        {
            var resultado = new ResultadoApi();
            try
            {
                var hojaDeRuta = _db.HojaRuta.Where(c => c.IdHojaRuta == id).FirstOrDefault();
                _db.HojaRuta.Remove(hojaDeRuta);
                _db.SaveChanges();

                resultado.Ok = true;
                resultado.Return = _db.HojaRuta.ToList();

                return resultado;
            }
            catch (System.Exception ex)
            {
                resultado.Ok = false;
                resultado.Error = "Hoja de Ruta no encontrada" + ex.Message;

                return resultado;
            }
        }


    }

}
