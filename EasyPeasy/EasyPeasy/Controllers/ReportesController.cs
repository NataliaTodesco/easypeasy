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
            Random random = new Random();
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
                            value = random.Next(total),
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
    }
}
