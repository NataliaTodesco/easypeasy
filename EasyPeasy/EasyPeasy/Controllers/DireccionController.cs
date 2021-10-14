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
    public class DireccionController:ControllerBase
    {
        private readonly EasyPeasyDBContext db = new EasyPeasyDBContext();

        [HttpPost]
        [Route("Direccion/RegistrarDireccion")]
        public ActionResult<ResultadoApi> AltaDireccion([FromBody] ComandoDireccion comando)
        {
            var resultado = new ResultadoApi();

            if (comando.idBarrio.Equals(""))
            {
                resultado.Ok = false;
                resultado.Error = "Ingrese Barrio";
                return resultado;
            }
            if (comando.idZona.Equals(""))
            {
                resultado.Ok = false;
                resultado.Error = "Ingrese Zona";
                return resultado;
            }

           
            if (comando.Descripcion.Equals(""))
            {
                resultado.Ok = false;
                resultado.Error = "Ingrese calle y altura";
                return resultado;
            }

            var direccion = new Barrio();
            direccion.IdBarrio=comando.idBarrio;
            direccion.Descripcion=comando.Descripcion;
            direccion.IdZona=comando.idZona;
            
            db.Barrios.Add(direccion);
            db.SaveChanges(); 
            resultado.Ok = true;

            return resultado;
        }
        }

    }


