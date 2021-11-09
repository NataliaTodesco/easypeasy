using System.Security.Cryptography.X509Certificates;
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
    public class DetalleEntregaController:ControllerBase
    {
        private readonly EasyPeasyDBContext _db = new EasyPeasyDBContext();

        //Obtener todos los detalles de entregas
        [HttpGet]
        [Route("DetalleEntrega/ObtenerDetallesEntrega")]
        public ActionResult<ResultadoApi> GetAll()
        {
            var Resultado = new ResultadoApi();
            try{
                Resultado.Ok = true;
                var detallesEntrega =  _db.DetalleEntregas.Include(x => x.IdRemitoNavigation)
                                .ToList();

                Resultado.Return=detallesEntrega;
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }
        }

        //Cargar nuevo detalle de entrega
        [HttpPost]
        [Route("DetalleEntrega/CargarDetalleEntrega")]
        public ActionResult<ResultadoApi> AltaDetalleEntrega([FromBody]ComandoDetalleEntrega comando)
        {
            var resultado = new ResultadoApi();

            var de = new DetalleEntrega();
            de.IdRemito = comando.IdRemito;
            de.HoraEntrega = comando.HoraEntrega;
            de.Firma = comando.Firma;
            de.Observaciones = comando.Observaciones;

            _db.DetalleEntregas.Add(de);
             //busco hoja de ruta q tiene el remito para buscar al transportista
            var remito=_db.Remitos.Include(x=>x.IdHojaRutaNavigation).FirstOrDefault(x=>x.IdRemito==comando.IdRemito);        
            
             var transportista=_db.Transportistas.FirstOrDefault(x=>x.IdTransportista==remito.IdHojaRutaNavigation.IdTransportista);
             //cambio disponibilidad de transportista responsable de la entrega a "disponible"
             //transportista.idDisponibilidad=1;            
            _db.SaveChanges();

            resultado.Ok = true;
            resultado.Return = de;

            return resultado;
        }
    }
}