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
    public class ArticuloController:ControllerBase
    {
        private readonly EasyPeasyDBContext _db = new EasyPeasyDBContext();

        [HttpGet]
        [Route("Producto/ObtenerProducto")]

        public ActionResult<ResultadoApi> Get()
        {
            var Resultado = new ResultadoApi();
            try{
                Resultado.Ok = true;
                Resultado.Return = _db.Productos .OrderBy(x => x.IdProducto).ToList();            
                      
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }

        [HttpGet]
        [Route("Producto/ObtenerUnProducto")]
        public ActionResult<ResultadoApi> Get(int id)
        {
            var Resultado = new ResultadoApi();

            try{
                Resultado.Ok = true;
                Resultado.Return = _db.Productos.FirstOrDefault(x => x.IdProducto == id);
              
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
