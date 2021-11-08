using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using EasyPeasy.Models;
using EasyPeasy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using api.Resultados;

namespace EasyPeasy.Controllers
{
    [ApiController]
    [EnableCors("postgres")]
    public class ReportesController : ControllerBase
    {
        private readonly EasyPeasyDBContext _db = new EasyPeasyDBContext();
        [HttpGet]
        [Route("/Reportes/EntregasPorTransportista")]
        public ActionResult<ResultadoApi> EntregasPorTransportista()
        {
            //Random random = new Random();
            //listado de transportistas
            var transportistasList = _db.Transportistas.ToList();
            var entregasList = new List<ReportesViewModel>();
            ResultadoApi resultadoApi = new ResultadoApi();
            try
            {
               if (transportistasList != null)
                {
                    int total = 100;
                    for (int i = 0; i < transportistasList.Count(); i++)
                    {
                        ReportesViewModel report = new ReportesViewModel()
                        {
                            text = transportistasList[i].Nombre,
                            // value = random.Next(total),
                            value=_db.HojaRuta.Where(x=>x.IdTransportista==transportistasList[i].IdTransportista).Count(),
                            color = String.Format("#{0:X6}", new Random().Next(0x1000000))
                        };
                        total -= report.value;
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
            //Random random = new Random();
            ResultadoApi resultadoApi = new ResultadoApi();
            try
            {
                // int total = 100;
                // var entregadas = random.Next(total);
                // var reprogramadas = random.Next(total - entregadas);
                // int[] estados = new int[2];
                var estados= new List<int>();
                estados.Add(_db.Remitos.Where(x=>x.IdEstado==3).Count());
                estados.Add(_db.Remitos.Where(x=>x.IdEstado==4).Count());

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
                var detalles= _db.Remitos.Include(x=>x.DetalleEntregas).ToList();  
                      
                resultadoApi.Ok = true;
                resultadoApi.Return = detalles;
                return resultadoApi;
            }

            catch (Exception ex)
            {
                resultadoApi.Ok = false;
                resultadoApi.Error = "Error " + ex.Message;
                return resultadoApi;
            }

        }
    }
}
