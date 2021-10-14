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
    public class RemitoController:ControllerBase
    {
        private readonly EasyPeasyDBContext _db = new EasyPeasyDBContext();

        //Obetener listado de remitos con estado pendiente ordenados por fecha de compra

        [HttpGet]
        [Route("Remito/ObtenerRemitosPendientes")]
        public ActionResult<ResultadoApi> Get()
        {
            var Resultado = new ResultadoApi();
            try{
                Resultado.Ok = true;
                var remitos =  _db.Remitos
                .Include(x=>x.IdClienteNavigation)
                        .Where(x => x.IdEstado == 1)
                        .OrderBy(x => x.FechaCompra)
                        .ToList();
                              
                Resultado.Return=remitos;
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }

        //Obtener todos los remitos
         [HttpGet]
        [Route("Remito/ObtenerRemitos")]
        public ActionResult<ResultadoApi> GetAll()
        {
            var Resultado = new ResultadoApi();
            try{
                Resultado.Ok = true;
                var remitos =  _db.Remitos.ToList();
                              
                Resultado.Return=remitos;
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }

        //Obtener remito segun id, mostrar detalles de cliente y productos
        [HttpGet]
        [Route("Remito/ObtenerRemito")]
        public ActionResult<ResultadoApi> Get(int id)
        {
            var Resultado = new ResultadoApi();

            try{
                Resultado.Ok = true;
                 Resultado.Return = _db.Remitos
                    .Include(x => x.IdClienteNavigation)
                    .ThenInclude(x => x.IdBarrioNavigation)
                    .ThenInclude(x => x.IdZonaNavigation)
                    .Include(x => x.ProductosXremitos)
                    .ThenInclude(x => x.IdProductoNavigation)
                    .FirstOrDefault(x => x.IdRemito == id);
              
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }

        [HttpPost]
        [Route("Remito/CargarRemito")]
        public ActionResult<ResultadoApi> AltaRemito([FromBody]ComandoRemito comando)
        {
            var resultado = new ResultadoApi();

            if(comando.FechaCompra.Equals(""))
            {
                resultado.Ok = false;
                resultado.Error = "Ingrese Fecha de Compra";
                return resultado;
            }

            if(comando.HoraEntregaPreferido.Equals(""))
            {
                resultado.Ok = false;
                resultado.Error = "Ingrese Hora de Entrega Preferida";
                return resultado;
            }

            var r = new Remito();
            r.FechaCompra = comando.FechaCompra;
            r.HoraEntregaPreferido = comando.HoraEntregaPreferido;
            r.IdEstado = comando.IdEstado;
            r.IdCliente = comando.IdCliente;
            r.IdHojaRuta = comando.IdHojaRuta;
         
            _db.Remitos.Add(r);
            _db.SaveChanges();
           
            resultado.Ok = true;
            resultado.Return = _db.Remitos.ToList();

            return resultado;
        }



        [HttpPut]
        [Route("Remito/Actualizar")]
        public ActionResult<ResultadoApi> Update([FromBody]ComandoRemito comando)
        {
            var resultado = new ResultadoApi();
           
            if(comando.FechaCompra.Equals(""))
            {
                resultado.Ok = false;
                resultado.Error = "Ingrese Fecha de Compra";
                return resultado;
            }

            if(comando.HoraEntregaPreferido.Equals(""))
            {
                resultado.Ok = false;
                resultado.Error = "Ingrese Hora de Entrega Preferida";
                return resultado;
            }

            var r = _db.Remitos.Where(c =>c.IdRemito == comando.IdRemito).FirstOrDefault();
            if(r != null)
            {
                r.FechaCompra = comando.FechaCompra;
                r.HoraEntregaPreferido = comando.HoraEntregaPreferido;
                r.IdEstado = comando.IdEstado;
                r.IdCliente = comando.IdCliente;
                r.IdHojaRuta = comando.IdHojaRuta;
                _db.Remitos.Update(r);
                _db.SaveChanges();
            }

            resultado.Ok = true;
            resultado.Return = _db.Remitos.ToList();;

            return resultado;
        }

        //get listado de remitos por  zona
        [HttpGet]
        [Route("Remito/ObtenerRemitosPorZona")]
        public ActionResult<ResultadoApi> GetRemitoPorZona(int idZona)
        {
            var Resultado = new ResultadoApi();
            try{
                Resultado.Ok = true;
                //busco remitos en estado pendiente que pertenezcan a la zona solicitada
                var remitos =  _db.Remitos
                        .Where(x => x.IdEstado == 1 &&
                               x.IdClienteNavigation.IdBarrioNavigation.IdZona==idZona)
                        .ToList();
                              
                Resultado.Return = remitos;
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }
    }

    
}
