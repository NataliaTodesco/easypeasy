using System;
using Microsoft.AspNetCore.Mvc;
using EasyPeasy.Models;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using api.Resultados;

namespace api.Controllers
{
    [ApiController]
    [EnableCors("postgres")]
    public class EstadoController:ControllerBase
    {
        private readonly EasyPeasyDBContext _db = new EasyPeasyDBContext();

        [HttpGet]
        [Route("Estado/ObtenerEstado")]

        public ActionResult<ResultadoApi> Get()
        {
            var Resultado = new ResultadoApi();
            try{
                Resultado.Ok = true;
                Resultado.Return = _db.Estados.ToList();            
                      
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
