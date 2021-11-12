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
    public class EntregasController : ControllerBase
    {
        private readonly EasyPeasyDBContext db = new EasyPeasyDBContext();

        [HttpPut]
        [Route("Entregas/ModificarEntregas")]

        public ActionResult<ResultadoApi> UpdateEntrega([FromBody] ComandoUpdateEntrega comando)
        {
            var resultado = new ResultadoApi();

            if (comando.Observaciones.Equals(""))
            {
                resultado.Ok = false;
                resultado.Error = "ingrese observacion";
                return resultado;
            }

            var entrega = db.DetalleEntregas.Where(c => c.IdDetalle == comando.IdDetalle).FirstOrDefault();
            if (entrega != null)
            {
                entrega.Observaciones = comando.Observaciones;
                db.DetalleEntregas.Update(entrega);
                db.SaveChanges();
            }
            resultado.Ok = true;

            return resultado;
        }
        [HttpGet]
        [Route("Entregas/ObtenerDetalleEntrega")]
        public ActionResult<ResultadoApi> getDetalleEntrega()
        {
            var resultado = new ResultadoApi();
            try
            {
                resultado.Ok = true;
                resultado.Return = db.DetalleEntregas.ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                resultado.Ok = false;

                resultado.Error = "Error al encontrar";

                return resultado;
            }
        }

        [HttpDelete]
        [Route("Entregas/DeleteEntrega")]
        public ActionResult<ResultadoApi> deleteById(int IdDetalle)
        {
            var resultado = new ResultadoApi();
            var entrega = db.DetalleEntregas.Where(c => c.IdDetalle == IdDetalle).FirstOrDefault();
            db.DetalleEntregas.Remove(entrega);
            db.SaveChanges();

            resultado.Ok = true;
  
            return resultado;



        }


    }

}


