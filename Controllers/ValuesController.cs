using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using locadora.banco;
using locadora.Models;
using System.Collections.Generic;

namespace locadora.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        //GET api/async
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var db = new AppDb(null))
            {
                await db.Connection.OpenAsync();
                var query = new VeiculoQuery(db);
                var result = await query.ListaVeiculoAsync();
                return new OkObjectResult(result);
            }
        }

        //GET api/async
        [HttpGet("{placa}")]
        public async Task<IActionResult> GetID(string placa)
        {
            using (var db = new AppDb(null))
            {
                await db.Connection.OpenAsync();
                var query = new VeiculoQuery(db);
                var result = await query.BuscaVeiculoIDAsync(placa);
                return new OkObjectResult(result);
            }
        }

        //POST api/async 
        [HttpPost()]
        public async Task<IActionResult> PostNew(locadora.models.Veiculo veiculonovo)
        {

            using (var db = new AppDb(null))
            {
                await db.Connection.OpenAsync();
                var query = new VeiculoQuery(db);
                var result = await query.InsereNovoVeiculoAsync(veiculonovo);
                return new OkObjectResult(result);
            }
        }

        //PUT api/async 
        [HttpPut()]
        public async Task<IActionResult> PUTAtualiza(locadora.models.Veiculo veiculonovo)
        {

            using (var db = new AppDb(null))
            {
                await db.Connection.OpenAsync();
                var query = new VeiculoQuery(db);
                var result = await query.AlteraVeiculoAsync(veiculonovo);
                return new OkObjectResult(result);
            }
        }

        //PUT api/async 
        [HttpDelete("{placa}")]
        public async Task<IActionResult> DeleteID(string placa)
        {

            using (var db = new AppDb(null))
            {
                await db.Connection.OpenAsync();
                var query = new VeiculoQuery(db);
                var result = await query.DeletaVeiculoAsync(placa);
                return new OkObjectResult(result);
            }
        }

    }
}



