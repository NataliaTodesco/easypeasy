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
using EasyPeasy.Models.DTO;

namespace api.Controllers
{
    [ApiController]
    [EnableCors("postgres")]
    public class RemXProdController : ControllerBase
    {
        private readonly EasyPeasyDBContext db = new EasyPeasyDBContext();

        [HttpGet]
        [Route("RemXProd/ObtenerRemXProd")]

        public ActionResult<ResultadoApi> Get()
        {
            var Resultado = new ResultadoApi();
            try
            {
                Resultado.Ok = true;
                //Resultado.Return = db.ProductosXremitos.ToList();            
                ListadoRemitosXProductos RXP = new ListadoRemitosXProductos();
                RXP.RemitosXProductos = (from rxp in db.ProductosXremitos
                                         select new DTORemitoXProducto
                                         {
                                             IdRemito = rxp.IdRemito,
                                             IdProducto = rxp.IdProducto,
                                             Cantidad = rxp.Cantidad.GetValueOrDefault(),
                                             Producto = (from p in db.Productos
                                                         where rxp.IdProducto == p.IdProducto
                                                         select new RProducto
                                                         {
                                                             IdProducto = p.IdProducto,
                                                             Descripcion = p.Descripcion
                                                         }
                                             ).FirstOrDefault(),
                                             Remito = (from r in db.Remitos
                                                       where rxp.IdRemito == r.IdRemito
                                                       select new DTORemito
                                                       {
                                                           IdRemito = r.IdRemito,
                                                           FechaCompra = r.FechaCompra,
                                                           HoraEntregaPreferido = r.HoraEntregaPreferido,
                                                           Estado = (from e in db.Estados
                                                                     where r.IdEstado == e.IdEstado
                                                                     select new REstado
                                                                     {
                                                                         IdEstado = r.IdEstado.GetValueOrDefault(),
                                                                         Descripcion = e.Descripcion
                                                                     }

                                                                          ).FirstOrDefault(),
                                                           Cliente = (from c in db.Clientes
                                                                      where r.IdCliente == c.IdCliente
                                                                      select new RCliente
                                                                      {
                                                                          IdCliente = c.IdCliente,
                                                                          Nombre = c.Nombre,
                                                                          Direccion = c.Direccion,
                                                                          Telefono = c.Telefono
                                                                      }
                                                              ).FirstOrDefault(),
                                                           HojaRuta = (from hr in db.HojaRuta
                                                                       where hr.IdHojaRuta == r.IdHojaRuta
                                                                       select new RHojaRuta
                                                                       {
                                                                           IdHojaRuta = hr.IdHojaRuta,
                                                                           Fecha = hr.Fecha,
                                                                           IdVehiculo = hr.IdVehiculo,
                                                                           IdTransportista = hr.IdTransportista
                                                                       }

                                                            ).FirstOrDefault(),
                                        //                    ProductosXRemitos = (from pxr in db.ProductosXremitos  
                                        //                     where pxr.IdRemito == r.IdRemito
                                        //                     select new RProductoXRemito{
                                        //                     Producto =(from p in db.Productos
                                        //                                 where p.IdProducto == pxr.IdProducto
                                        //                                 select new RProducto{
                                        //                                     IdProducto = pxr.IdProducto,
                                        //                                     Descripcion = p.Descripcion
                                        //                                 }    
                                        //                     ).FirstOrDefault(),
                                        //                     Cantidad = pxr.Cantidad                                                             
                                        //                     }
                                        // ).ToList()
                                                       }).FirstOrDefault()
                                         }
                ).ToList();
                Resultado.Return = RXP;
                return Resultado;
            }
            catch (Exception ex)
            {
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }

        [HttpPost]
        [Route("RemXProd/RegistrarRemXProd")]
        public ActionResult<ResultadoApi> AltaDireccion([FromBody] ComandoRemXProd comando)
        {
            var resultado = new ResultadoApi();

            if (comando.IdProducto.Equals(0))
            {
                resultado.Ok = false;
                resultado.Error = "Ingrese IdProducto";
                return resultado;
            }
            if (comando.IdRemito.Equals(""))
            {
                resultado.Ok = false;
                resultado.Error = "Ingrese IdRemito";
                return resultado;
            }

            var x = new ProductosXremito();
            x.IdProducto = comando.IdProducto;
            x.IdRemito = comando.IdRemito;
            x.Cantidad = comando.Cantidad;

            db.ProductosXremitos.Add(x);
            db.SaveChanges();
            resultado.Ok = true;

            return resultado;
        }

        [HttpDelete]
        [Route("/RemXProd/EliminarRemXProd")]
        public ActionResult<ResultadoApi> Delete(int id)
        {
            var resultado = new ResultadoApi();
            try
            {
                var rem = db.ProductosXremitos.Where(c => c.IdRemito == id);
                foreach (var i in rem)
                {
                    db.ProductosXremitos.Remove(i);
                }

                db.SaveChanges();
                resultado.Ok = true;
                resultado.Return = db.ProductosXremitos.ToList();

                return resultado;
            }
            catch (System.Exception ex)
            {
                resultado.Ok = false;
                resultado.Error = "Remito no encontrado" + ex.Message;

                return resultado;
            }
        }
    }

}


