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

namespace api.Controllers
{
    [ApiController]
    [EnableCors("postgres")]
    public class RemitoController:ControllerBase
    {
        private readonly EasyPeasyDBContext _db= new EasyPeasyDBContext();

        //Obetener listado de remitos con estado pendiente ordenados por fecha de compra

        [HttpGet]
        [Route("Remito/ObtenerRemitos")]
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
    }
}
