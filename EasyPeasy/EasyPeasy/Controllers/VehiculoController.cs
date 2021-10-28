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
using api.Resultados;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using EasyPeasy.Models;
using EasyPeasy.Comandos;

namespace api.Controllers
{
    [ApiController]
    [EnableCors("postgres")]
    public class VehiculoController:ControllerBase
    {
         private readonly EasyPeasyDBContext _db = new EasyPeasyDBContext();

        [HttpGet]
        [Route("Vehiculo/ObtenerVehiculos")]

        public ActionResult<ResultadoApi> Get()
        {
            var Resultado = new ResultadoApi();
            try{
                Resultado.Ok = true;
                Resultado.Return = _db.Vehiculos
                                  .ToList();            
                      
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }
        [HttpPost]
        [Route("Vehiculo/RegistrarVehiculo")]
        public ActionResult<ResultadoApi> post([FromBody]ComandoVehiculo comando){
            var Resultado = new ResultadoApi();
            var v = new Vehiculo();
            try
            {
                v.IdVehiculo = 0;
                v.Patente=comando.Patente;
                v.Color=comando.Color;
                v.Modelo=comando.Modelo;
                _db.Vehiculos.Add(v);
                _db.SaveChanges();
                Resultado.Ok = true;
                Resultado.Return = v;
                return Resultado;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                Resultado.Ok=false;
                Resultado.Error = e.Message;
                return Resultado;
            }
        }

    }
}
