using System.Net.WebSockets;
using System.Globalization;
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
using EasyPeasy.Comandos;
using EasyPeasy.Models;

namespace api.Controllers
{
    [ApiController]
    [EnableCors("postgres")]
    public class DireccionController : ControllerBase
    {
        private readonly EasyPeasyDBContext db = new EasyPeasyDBContext();

        [HttpGet]
        [Route("Direccion/ConsultarDireccion")]
        public ActionResult<ResultadoApi> ObtenerDireccion()
        {
            var resultado = new ResultadoApi();
            try
            {
                resultado.Ok = true;
            
                
                resultado.Return=db.Clientes.Select(x=>x.Direccion);
                return resultado;


                
            }
            catch (Exception ex)
            {
                resultado.Ok = false;
                resultado.Error = "Error al listar direcciones" + ex;

                return resultado;
            }
        }



        [HttpPut]
        [Route("Direccion/ModificarDireccion")]
        public ActionResult<ResultadoApi> ModificarDireccion([FromBody] ComandoDireccion comando)
        {
          var resultado = new ResultadoApi();

            if (comando.Direccion.Equals(""))
            {
                resultado.Ok = false;
                resultado.Error = "ingrese dirección";
                return resultado;
            }

            var direccion = db.Clientes.Where(c => c.IdCliente == comando.IdCliente).FirstOrDefault();
            if (direccion != null)
            {
                direccion.Direccion = comando.Direccion;
                db.Clientes.Update(direccion);
                db.SaveChanges();
            }
            resultado.Ok = true;

            return resultado;
        } 
    }

}


