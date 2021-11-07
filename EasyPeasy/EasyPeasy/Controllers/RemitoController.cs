using System.Data;
using System;
using Microsoft.AspNetCore.Mvc;
using api.Resultados;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using EasyPeasy.Models;
using EasyPeasy.Models.DTO;
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
                // var remitos =  _db.Remitos.Include(x => x.IdClienteNavigation)
                //                 .ThenInclude(x => x.IdBarrioNavigation)
                //                 .ThenInclude(x => x.IdZonaNavigation)
                //                 .Include(x => x.ProductosXremitos)
                //                 .ThenInclude(x => x.IdProductoNavigation)
                //                 .ToList();
                ListadoRemitos remitos = new ListadoRemitos();
                remitos.Remitos= (from r in _db.Remitos 
                                    select new DTORemito{
                                        IdRemito = r.IdRemito ,
                                        FechaCompra = r.FechaCompra ,
                                        HoraEntregaPreferido = r.HoraEntregaPreferido ,
                                        Estado = (from e in _db.Estados
                                                    where r.IdEstado == e.IdEstado
                                                    select new REstado{
                                                        IdEstado = r.IdEstado.GetValueOrDefault() ,
                                                        Descripcion = e.Descripcion
                                                    }

                                        ) .FirstOrDefault(),
                                        Cliente = (from c in _db.Clientes
                                                    where  r.IdCliente == c.IdCliente
                                                    select new RCliente{
                                                        IdCliente = c.IdCliente ,
                                                        Nombre = c.Nombre ,
                                                        Direccion = c.Direccion ,
                                                        Telefono = c.Telefono 
                                                    }
                                        ).FirstOrDefault(),
                                        HojaRuta = (from hr in _db.HojaRuta
                                                    where hr.IdHojaRuta == r.IdHojaRuta
                                                    select new RHojaRuta{
                                                    IdHojaRuta = hr.IdHojaRuta ,
                                                    Fecha = hr.Fecha ,
                                                    IdVehiculo = hr.IdVehiculo ,
                                                    IdTransportista = hr.IdTransportista    
                                                    }

                                        ).FirstOrDefault() ,
                                        ProductosXRemitos = (from pxr in _db.ProductosXremitos
                                                            where pxr.IdRemito == r.IdRemito
                                                            select new RProductoXRemito{
                                                            Producto =(from p in _db.Productos
                                                                        where p.IdProducto == pxr.IdProducto
                                                                        select new RProducto{
                                                                            IdProducto = pxr.IdProducto,
                                                                            Descripcion = p.Descripcion
                                                                        }    
                                                            ).FirstOrDefault(),
                                                            Cantidad = pxr.Cantidad                                                             
                                                            }
                                        ).ToList()
                                    }).ToList();
                Resultado.Return=remitos;
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }
        //Obtener todos los remitos y el estado y transportista
        [HttpGet]
        [Route("Remito/ObtenerDetallesRemitos")]
        public ActionResult<ResultadoApi> GetRemitosDetails()
        {
            var Resultado = new ResultadoApi();
            try{
                Resultado.Ok = true;
                var remitos =  _db.Remitos.Include(x => x.IdEstadoNavigation)
                                .Include(x => x.IdHojaRutaNavigation)
                                .ThenInclude(x => x.IdTransportistaNavigation)
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
                //  Resultado.Return = _db.Remitos
                //     .Include(x => x.IdClienteNavigation)
                //     .ThenInclude(x => x.IdBarrioNavigation)
                //     .ThenInclude(x => x.IdZonaNavigation)
                //     .Include(x => x.ProductosXremitos)
                //     .ThenInclude(x => x.IdProductoNavigation)
                //     .FirstOrDefault(x => x.IdRemito == id);
                 DTORemito remito = new DTORemito();
                remito= (from r in _db.Remitos 
                        where r.IdRemito == id
                                    select new DTORemito{
                                        IdRemito = r.IdRemito ,
                                        FechaCompra = r.FechaCompra ,
                                        HoraEntregaPreferido = r.HoraEntregaPreferido ,
                                        Estado = (from e in _db.Estados
                                                    where r.IdEstado == e.IdEstado
                                                    select new REstado{
                                                        IdEstado = r.IdEstado.GetValueOrDefault() ,
                                                        Descripcion = e.Descripcion
                                                    }

                                        ) .FirstOrDefault(),
                                        Cliente = (from c in _db.Clientes
                                                    where  r.IdCliente == c.IdCliente
                                                    select new RCliente{
                                                        IdCliente = c.IdCliente ,
                                                        Nombre = c.Nombre ,
                                                        Direccion = c.Direccion ,
                                                        Telefono = c.Telefono 
                                                    }
                                        ).FirstOrDefault(),
                                        HojaRuta = (from hr in _db.HojaRuta
                                                    where hr.IdHojaRuta == r.IdHojaRuta
                                                    select new RHojaRuta{
                                                    IdHojaRuta = hr.IdHojaRuta ,
                                                    Fecha = hr.Fecha ,
                                                    IdVehiculo = hr.IdVehiculo ,
                                                    IdTransportista = hr.IdTransportista    
                                                    }

                                        ).FirstOrDefault() ,
                                        ProductosXRemitos = (from pxr in _db.ProductosXremitos
                                                            where pxr.IdRemito == r.IdRemito
                                                            select new RProductoXRemito{
                                                            Producto =(from p in _db.Productos
                                                                        where p.IdProducto == pxr.IdProducto
                                                                        select new RProducto{
                                                                            IdProducto = pxr.IdProducto,
                                                                            Descripcion = p.Descripcion
                                                                        }    
                                                            ).FirstOrDefault(),
                                                            Cantidad = pxr.Cantidad                                                             
                                                            }
                                        ).ToList()
                                    }).FirstOrDefault();
                Resultado.Return=remito;
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

            var r = new Remito();
            r.FechaCompra = comando.FechaCompra;
            r.HoraEntregaPreferido = comando.HoraEntregaPreferido;
            r.IdEstado = comando.IdEstado;
            r.IdCliente = comando.IdCliente;
            if (comando.IdHojaRuta!=0){
                r.IdHojaRuta = comando.IdHojaRuta;
            }
          
            _db.Remitos.Add(r);
            _db.SaveChanges();
           
            resultado.Ok = true;
            resultado.Return = r;

            return resultado;
        }



        [HttpPut]
        [Route("Remito/Actualizar")]
        public ActionResult<ResultadoApi> Update([FromBody]ComandoRemito comando)
        {
            var resultado = new ResultadoApi();
           

            var r = _db.Remitos.Where(c =>c.IdRemito == comando.IdRemito).FirstOrDefault();
            if(r != null)
            {
                r.FechaCompra = comando.FechaCompra;
                r.HoraEntregaPreferido = comando.HoraEntregaPreferido;
                r.IdEstado = comando.IdEstado;
                r.IdCliente = comando.IdCliente;
      
                _db.Remitos.Update(r);
                _db.SaveChanges();

                resultado.Ok = true;
                resultado.Return = _db.Remitos.ToList();;
            }
            else {
                resultado.Ok = false;
                resultado.Error = "Elemento Nulo";
            }

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
                // var remitos =  _db.Remitos
                //         .Where(x => x.IdEstado == 1 &&
                //                x.IdClienteNavigation.IdBarrioNavigation.IdZona==idZona)
                //         .Include(x => x.IdClienteNavigation)
                //         .ThenInclude(x => x.IdBarrioNavigation)
                //         .ThenInclude(x => x.IdZonaNavigation)
                //         .Include(x => x.ProductosXremitos)
                //         .ThenInclude(x => x.IdProductoNavigation)
                //         .ToList();
                 ListadoRemitos remitos = new ListadoRemitos();
                remitos.Remitos= (from r in _db.Remitos 
                                    where r.IdEstado == 1 &&
                                    r.IdClienteNavigation.IdBarrioNavigation.IdZona == idZona
                                    select new DTORemito{
                                        IdRemito = r.IdRemito ,
                                        FechaCompra = r.FechaCompra ,
                                        HoraEntregaPreferido = r.HoraEntregaPreferido ,
                                        Estado = (from e in _db.Estados
                                                    where r.IdEstado == e.IdEstado
                                                    select new REstado{
                                                        IdEstado = r.IdEstado.GetValueOrDefault() ,
                                                        Descripcion = e.Descripcion
                                                    }

                                        ) .FirstOrDefault(),
                                        Cliente = (from c in _db.Clientes
                                                    where  r.IdCliente == c.IdCliente
                                                    select new RCliente{
                                                        IdCliente = c.IdCliente ,
                                                        Nombre = c.Nombre ,
                                                        Direccion = c.Direccion ,
                                                        Telefono = c.Telefono 
                                                    }
                                        ).FirstOrDefault(),
                                        HojaRuta = (from hr in _db.HojaRuta
                                                    where hr.IdHojaRuta == r.IdHojaRuta
                                                    select new RHojaRuta{
                                                    IdHojaRuta = hr.IdHojaRuta ,
                                                    Fecha = hr.Fecha ,
                                                    IdVehiculo = hr.IdVehiculo ,
                                                    IdTransportista = hr.IdTransportista    
                                                    }

                                        ).FirstOrDefault() ,
                                        ProductosXRemitos = (from pxr in _db.ProductosXremitos
                                                            where pxr.IdRemito == r.IdRemito
                                                            select new RProductoXRemito{
                                                            Producto =(from p in _db.Productos
                                                                        where p.IdProducto == pxr.IdProducto
                                                                        select new RProducto{
                                                                            IdProducto = pxr.IdProducto,
                                                                            Descripcion = p.Descripcion
                                                                        }    
                                                            ).FirstOrDefault(),
                                                            Cantidad = pxr.Cantidad                                                             
                                                            }
                                        ).ToList()
                                    }).ToList();
                Resultado.Return=remitos;
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }

        [HttpDelete]
        [Route("/Remito/EliminarRemito")]
        public ActionResult<ResultadoApi> Delete(int id)
        {
            var resultado = new ResultadoApi();
            try
            {
                var rem = _db.Remitos.Where(c => c.IdRemito == id).FirstOrDefault();
                _db.Remitos.Remove(rem);
                _db.SaveChanges();

                resultado.Ok = true;
                resultado.Return = _db.Remitos.ToList();

                return resultado;
            }
            catch (System.Exception ex)
            {  
                resultado.Ok = false;
                resultado.Error = "Remito no encontrado" + ex.Message;

                return resultado;
            }
        }

        [HttpGet]
        [Route("Remito/ObtenerRemitoXFecha")]
        public ActionResult<ResultadoApi> Get(DateTime fecha)
        {
            var Resultado = new ResultadoApi();

            try{
                Resultado.Ok = true;
                //  Resultado.Return = _db.Remitos
                //     .Include(x => x.IdClienteNavigation)
                //     .ThenInclude(x => x.IdBarrioNavigation)
                //     .ThenInclude(x => x.IdZonaNavigation)
                //     .Include(x => x.ProductosXremitos)
                //     .ThenInclude(x => x.IdProductoNavigation)
                //     .Where(x => x.FechaCompra == fecha)
                //     .ToList();
               ListadoRemitos remitos = new ListadoRemitos();
                remitos.Remitos= (from r in _db.Remitos 
                                    where r.FechaCompra == fecha
                                    select new DTORemito{
                                        IdRemito = r.IdRemito ,
                                        FechaCompra = r.FechaCompra ,
                                        HoraEntregaPreferido = r.HoraEntregaPreferido ,
                                        Estado = (from e in _db.Estados
                                                    where r.IdEstado == e.IdEstado
                                                    select new REstado{
                                                        IdEstado = r.IdEstado.GetValueOrDefault() ,
                                                        Descripcion = e.Descripcion
                                                    }

                                        ) .FirstOrDefault(),
                                        Cliente = (from c in _db.Clientes
                                                    where  r.IdCliente == c.IdCliente
                                                    select new RCliente{
                                                        IdCliente = c.IdCliente ,
                                                        Nombre = c.Nombre ,
                                                        Direccion = c.Direccion ,
                                                        Telefono = c.Telefono 
                                                    }
                                        ).FirstOrDefault(),
                                        HojaRuta = (from hr in _db.HojaRuta
                                                    where hr.IdHojaRuta == r.IdHojaRuta
                                                    select new RHojaRuta{
                                                    IdHojaRuta = hr.IdHojaRuta ,
                                                    Fecha = hr.Fecha ,
                                                    IdVehiculo = hr.IdVehiculo ,
                                                    IdTransportista = hr.IdTransportista    
                                                    }

                                        ).FirstOrDefault() ,
                                        ProductosXRemitos = (from pxr in _db.ProductosXremitos
                                                            where pxr.IdRemito == r.IdRemito
                                                            select new RProductoXRemito{
                                                            Producto =(from p in _db.Productos
                                                                        where p.IdProducto == pxr.IdProducto
                                                                        select new RProducto{
                                                                            IdProducto = pxr.IdProducto,
                                                                            Descripcion = p.Descripcion
                                                                        }    
                                                            ).FirstOrDefault(),
                                                            Cantidad = pxr.Cantidad                                                             
                                                            }
                                        ).ToList()
                                    }).ToList();
                Resultado.Return=remitos;
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }

        [HttpGet]
        [Route("Remito/ObtenerRemitoXEstado")]
        public ActionResult<ResultadoApi> Get2(int estado)
        {
            var Resultado = new ResultadoApi();

            try{
                Resultado.Ok = true;
                //  Resultado.Return = _db.Remitos
                //     .Include(x => x.IdClienteNavigation)
                //     .ThenInclude(x => x.IdBarrioNavigation)
                //     .ThenInclude(x => x.IdZonaNavigation)
                //     .Include(x => x.ProductosXremitos)
                //     .ThenInclude(x => x.IdProductoNavigation)
                //     .Where(x => x.IdEstado == estado)
                //     .ToList();
               ListadoRemitos remitos = new ListadoRemitos();
                remitos.Remitos= (from r in _db.Remitos 
                                    where r.IdEstado == estado
                                    select new DTORemito{
                                        IdRemito = r.IdRemito ,
                                        FechaCompra = r.FechaCompra ,
                                        HoraEntregaPreferido = r.HoraEntregaPreferido ,
                                        Estado = (from e in _db.Estados
                                                    where r.IdEstado == e.IdEstado
                                                    select new REstado{
                                                        IdEstado = r.IdEstado.GetValueOrDefault() ,
                                                        Descripcion = e.Descripcion
                                                    }

                                        ) .FirstOrDefault(),
                                        Cliente = (from c in _db.Clientes
                                                    where  r.IdCliente == c.IdCliente
                                                    select new RCliente{
                                                        IdCliente = c.IdCliente ,
                                                        Nombre = c.Nombre ,
                                                        Direccion = c.Direccion ,
                                                        Telefono = c.Telefono 
                                                    }
                                        ).FirstOrDefault(),
                                        HojaRuta = (from hr in _db.HojaRuta
                                                    where hr.IdHojaRuta == r.IdHojaRuta
                                                    select new RHojaRuta{
                                                    IdHojaRuta = hr.IdHojaRuta ,
                                                    Fecha = hr.Fecha ,
                                                    IdVehiculo = hr.IdVehiculo ,
                                                    IdTransportista = hr.IdTransportista    
                                                    }

                                        ).FirstOrDefault() ,
                                        ProductosXRemitos = (from pxr in _db.ProductosXremitos
                                                            where pxr.IdRemito == r.IdRemito
                                                            select new RProductoXRemito{
                                                            Producto =(from p in _db.Productos
                                                                        where p.IdProducto == pxr.IdProducto
                                                                        select new RProducto{
                                                                            IdProducto = pxr.IdProducto,
                                                                            Descripcion = p.Descripcion
                                                                        }    
                                                            ).FirstOrDefault(),
                                                            Cantidad = pxr.Cantidad                                                             
                                                            }
                                        ).ToList()
                                    }).ToList();
                Resultado.Return=remitos;
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "Error " + ex.Message;
                return Resultado;
            }

        }

        [HttpGet]
        [Route("Remito/ObtenerRemitoXFechaYEstado")]
        public ActionResult<ResultadoApi> Get2(int estado,DateTime fecha)
        {
            var Resultado = new ResultadoApi();

            try{
                Resultado.Ok = true;
                //  Resultado.Return = _db.Remitos
                //     .Include(x => x.IdClienteNavigation)
                //     .ThenInclude(x => x.IdBarrioNavigation)
                //     .ThenInclude(x => x.IdZonaNavigation)
                //     .Include(x => x.ProductosXremitos)
                //     .ThenInclude(x => x.IdProductoNavigation)
                //     .Where(x => x.IdEstado == estado && x.FechaCompra == fecha)
                //     .ToList();
               ListadoRemitos remitos = new ListadoRemitos();
                remitos.Remitos= (from r in _db.Remitos 
                                    where r.IdEstado == estado && r.FechaCompra == fecha
                                    select new DTORemito{
                                        IdRemito = r.IdRemito ,
                                        FechaCompra = r.FechaCompra ,
                                        HoraEntregaPreferido = r.HoraEntregaPreferido ,
                                        Estado = (from e in _db.Estados
                                                    where r.IdEstado == e.IdEstado
                                                    select new REstado{
                                                        IdEstado = r.IdEstado.GetValueOrDefault() ,
                                                        Descripcion = e.Descripcion
                                                    }

                                        ) .FirstOrDefault(),
                                        Cliente = (from c in _db.Clientes
                                                    where  r.IdCliente == c.IdCliente
                                                    select new RCliente{
                                                        IdCliente = c.IdCliente ,
                                                        Nombre = c.Nombre ,
                                                        Direccion = c.Direccion ,
                                                        Telefono = c.Telefono 
                                                    }
                                        ).FirstOrDefault(),
                                        HojaRuta = (from hr in _db.HojaRuta
                                                    where hr.IdHojaRuta == r.IdHojaRuta
                                                    select new RHojaRuta{
                                                    IdHojaRuta = hr.IdHojaRuta ,
                                                    Fecha = hr.Fecha ,
                                                    IdVehiculo = hr.IdVehiculo ,
                                                    IdTransportista = hr.IdTransportista    
                                                    }

                                        ).FirstOrDefault() ,
                                        ProductosXRemitos = (from pxr in _db.ProductosXremitos
                                                            where pxr.IdRemito == r.IdRemito
                                                            select new RProductoXRemito{
                                                            Producto =(from p in _db.Productos
                                                                        where p.IdProducto == pxr.IdProducto
                                                                        select new RProducto{
                                                                            IdProducto = pxr.IdProducto,
                                                                            Descripcion = p.Descripcion
                                                                        }    
                                                            ).FirstOrDefault(),
                                                            Cantidad = pxr.Cantidad                                                             
                                                            }
                                        ).ToList()
                                    }).ToList();
                Resultado.Return=remitos;
                return Resultado;
            }
            catch(Exception ex){
                Resultado.Ok = false;
                Resultado.Error = "No se encontraron remitos con el dia y estado especificado";
                return Resultado;
            }

        }

        // [HttpPut]
        // [Route("Remito/ActualizarEstado")]
        // public ActionResult<ResultadoApi> Update(int id)
        // {
        //     var resultado = new ResultadoApi();

        //     var r = _db.Remitos.Where(c => c.IdRemito == id).FirstOrDefault();
            
        //     if(r != null)
        //     {
        //         r.IdEstado = 3;

        //         _db.Remitos.Update(r);
        //         _db.SaveChanges();

        //         resultado.Ok = true;
        //         resultado.Return = _db.Remitos.ToList();
        //     }
        //     else {
        //         resultado.Ok = false;
        //         resultado.Error = "Elemento Nulo";
        //     }

        //     return resultado;
        // }

        [HttpPut]
        [Route("Remito/CambiarHojaRuta")]
        public ActionResult<ResultadoApi> CambiarHR(int idRemito, int idHojaRuta){
            ResultadoApi resultado = new ResultadoApi();
            Remito remito = _db.Remitos.Where(r=>r.IdRemito == idRemito).FirstOrDefault();
            HojaRuta hojaruta = _db.HojaRuta.Where(hr=>hr.IdHojaRuta == idHojaRuta).FirstOrDefault();
            if (remito != null && hojaruta != null){
                remito.IdHojaRuta = idHojaRuta;
                _db.Remitos.Update(remito);
                _db.SaveChanges();
                resultado.Ok = true;
                resultado.Return=remito;
                return resultado;
            }
            else{
                resultado.Ok = false;
                resultado.Error = "El remito que intentas asignar y/o la hoja de ruta a la que quieres asignarlo no existe";
                return resultado;
            }
        }
    }
}
