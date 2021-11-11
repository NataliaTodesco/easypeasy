using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using EasyPeasy.Models;
using EasyPeasy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using api.Resultados;
using Microsoft.EntityFrameworkCore;
using EasyPeasy.Models.DTO;

namespace EasyPeasy.Controllers
{
    [ApiController]
    [EnableCors("postgres")]
    public class ReportesController : ControllerBase
    {
        private readonly EasyPeasyDBContext _db = new EasyPeasyDBContext();

        [HttpGet]
        [Route("/Reportes/EntregasPorTransportista")]
        public ActionResult<ResultadoApi> EntregasPorTransportista(int id)
        {
            //Random random = new Random();
            //listado de transportistas
            var transportistasList = _db.Transportistas
                                    .Include(x=>x.HojaRuta)
                                    .ThenInclude(x=>x.Remitos)
                                    .ToList();
       

            var entregasList = new List<ReportesViewModel>();
            ResultadoApi resultadoApi = new ResultadoApi();
            try
            {
               if (transportistasList != null)
                {
                    int total = _db.Remitos.Where(x=>x.IdEstado==id).Count();
                    for (int i = 0; i < transportistasList.Count(); i++)
                    {    var entregas=0;
                        foreach(var e in transportistasList[i].HojaRuta){                       
                           entregas+=e.Remitos.Where(x=>x.IdEstado==id).Count();
                        }
                             
                        ReportesViewModel report = new ReportesViewModel()
                        {
                            total = total,
                            totalPorTransportista = entregas,
                            text = transportistasList[i].Nombre,                          
                            value=(entregas*100)/total,
                            color = String.Format("#{0:X6}", new Random().Next(0x1000000))
                        };
                       // total -= report.value;
                        entregasList.Add(report);
                    }

                }
                resultadoApi.Return = entregasList;
                return resultadoApi;
            }

            catch (Exception ex)
            {
                resultadoApi.Ok = false;
                resultadoApi.Error = "Error " + ex.Message;
                return resultadoApi;
            }
        }
        

        [HttpGet]
        [Route("/Reportes/EstadoEntregas")]
        public ActionResult<ResultadoApi> EstadoEntregas()
        {
            ResultadoApi resultadoApi = new ResultadoApi();
            try
            {
                var estados= new List<int>();
                estados.Add(_db.Remitos.Where(x=>x.IdEstado==3).Count());
                estados.Add(_db.Remitos.Where(x=>x.IdEstado==4).Count());
                estados.Add(_db.Remitos.Where(x=>x.IdEstado==5).Count());

                resultadoApi.Ok = true;
                resultadoApi.Return = estados;
                return resultadoApi;
            }

            catch (Exception ex)
            {
                resultadoApi.Ok = false;
                resultadoApi.Error = "Error " + ex.Message;
                return resultadoApi;
            }

        }


        [HttpGet]
        [Route("/Reportes/DetalleEntregas")]
        public ActionResult<ResultadoApi> DetalleEntregas()
        {
            ResultadoApi resultadoApi = new ResultadoApi();
            try
            {  
                Detalles dtodetalles = new Detalles();
                dtodetalles.DTOdetalleEntregas=(from dr in _db.DetalleEntregas
                            select new DTOdetalleEntregas{
                                 Id = dr.IdDetalle,
                                 IdRemito = dr.IdRemito,
                                 HoraEntrega = dr.HoraEntrega,
                                 Observaciones = dr.Observaciones,
                                 Remitos = (
                                         from r in _db.Remitos
                                         where r.IdRemito==dr.IdRemito 
                                         select new DRemito
                                         {
                                             IdRemito = r.IdRemito,
                                             FechaCompra = r.FechaCompra,
                                             IdEstado = r.IdEstado,
                                             HojaRuta = (
                                                 from h in _db.HojaRuta
                                                 where h.IdHojaRuta==r.IdHojaRuta
                                                 select new DHojaRuta{
                                                     Id=h.IdHojaRuta,
                                                     Fecha=h.Fecha,
                                                     IdTransportista=h.IdTransportista
                                                 }
                                             ).FirstOrDefault(),
                            }).ToList(),
                            }).ToList();
                      
                resultadoApi.Ok = true;
                resultadoApi.Return = dtodetalles;
                return resultadoApi;
            }

            catch (Exception ex)
            {
                resultadoApi.Ok = false;
                resultadoApi.Error = "Error " + ex.Message;
                return resultadoApi;
            }

        }


        // [HttpGet]
        // [Route("/Reportes/EntregasPorTransportista")]
        // public ActionResult<ResultadoApi> EntregasPorTransportista()
        // {
        //     //listado de transportistas
        //     var transportistasList = _db.Transportistas
        //                             .Include(x=>x.HojaRuta)
        //                             .ThenInclude(x=>x.Remitos)
        //                             .ToList();
       

        //     var entregasList = new List<ReportesViewModel>();
        //     ResultadoApi resultadoApi = new ResultadoApi();
        //     try
        //     {
        //        if (transportistasList != null)
        //         {
        //             int total = 100;
        //             for (int i = 0; i < transportistasList.Count(); i++)
        //             {    var entregas=0;
        //                 foreach(var e in transportistasList[i].HojaRuta){
        //                     //estado distinto a "en proceso"(2) y "pendiente"(1)
        //                    entregas+=e.Remitos.Where(x=>x.IdEstado!=2 && x.IdEstado!=1).Count();
        //                 }
                             
        //                 ReportesViewModel report = new ReportesViewModel()
        //                 {
        //                     text = transportistasList[i].Nombre,                          
        //                     value=entregas,
        //                     color = String.Format("#{0:X6}", new Random().Next(0x1000000))
        //                 };
        //                 total -= report.value;
        //                 entregasList.Add(report);
        //             }

        //         }
        //         resultadoApi.Return = entregasList;
        //         return resultadoApi;
        //     }

        //     catch (Exception ex)
        //     {
        //         resultadoApi.Ok = false;
        //         resultadoApi.Error = "Error " + ex.Message;
        //         return resultadoApi;
        //     }
        // }

    }
}
