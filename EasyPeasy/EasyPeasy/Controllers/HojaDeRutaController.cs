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
        [ValidateAntiForgeryToken] 
        [Route("HojaDeRuta/Upsert")]
        public ActionResult<ResultadoApi> Upsert (int? Id){

            var Resultado = new ResultadoApi();
            try{

                    if (Id == null)//crear nueva hoja de ruta
                    {            
                        Resultado.Ok = true;
                        Resultado.Return = new HojaRuta();
                        return Resultado;
                    }
                    else{//busco hoja de ruta por id
                            Resultado.Ok = true;
                            Resultado.Return =  _db.HojaRuta
                                            .Include(x=>x.Remitos)
                                            .Include(x=>x.IdTransportistaNavigation)
                                            .Include(x=>x.IdVehiculoNavigation)
                                            .FirstOrDefault(x => x.IdHojaRuta == Id);
                                            return Resultado;

                        }
                }
                 catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }
            
    

        }
    }

}
