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
    public class HojaDeRutaController:ControllerBase
    {
        private readonly EasyPeasyDBContext _db = new EasyPeasyDBContext();

        [HttpGet]
        [Route("HojaDeRuta/ObtenerHojasDeRuta")]

        public ActionResult<ResultadoApi> Get()
        {
            var Resultado = new ResultadoApi();
            try{
                Resultado.Ok = true;
                Resultado.Return = _db.HojaRuta
                                  .Include(x => x.IdTransportistaNavigation)
                                  .Include(x => x.IdVehiculoNavigation)
                                  .Include(x => x.Remitos)
                                  .ThenInclude(x => x.IdClienteNavigation)
                                  .ThenInclude(x => x.IdBarrioNavigation)
                                  .ThenInclude(x => x.IdZonaNavigation)
                                  .ToList();            
                      
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }

        //update/create get
        [HttpGet]        
        [Route("HojaDeRuta/Upsert")]
        public ActionResult<ResultadoApi> Upsert (int? Id){

            var Resultado = new ResultadoApi();
            try{
                     Resultado.Ok = true;
                    Resultado.Return =  _db.HojaRuta
                                         .Include(x=>x.Remitos)
                                         .FirstOrDefault(x => x.IdHojaRuta == Id);
                                        return Resultado;
                        
                }
                 catch(Exception ex){
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
                            && x.IdEstado == 1).ToList();
            //agrego hoja de ruta
            _db.HojaRuta.Add(hojaRuta);
            //cambio el estado de los remitos de "1"(pendiente) a "2"(en proceso)
            foreach (var item in hojaRuta.Remitos)
            {
                item.IdEstado = 2;
            }
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
                Resultado.Ok = true;
                Resultado.Return = _db.HojaRuta
                                   .Include(x => x.Remitos)
                                   .ThenInclude(x => x.IdClienteNavigation)
                                   .ThenInclude(x => x.IdBarrioNavigation)
                                   .ThenInclude(x => x.IdZonaNavigation)
                                   .Include(x => x.IdTransportistaNavigation)
                                   .Include(x => x.IdVehiculoNavigation)
                                   .FirstOrDefault(x => x.IdTransportistaNavigation.Legajo == legajo);
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
        public ActionResult<ResultadoApi> Update([FromBody]ComandoHojaRuta comando)
        {
            var resultado = new ResultadoApi();
           

            var hojaDeRuta = _db.HojaRuta.FirstOrDefault(x=>x.IdHojaRuta==comando.IdHojaRuta);
            if(hojaDeRuta != null)
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


    }

}
