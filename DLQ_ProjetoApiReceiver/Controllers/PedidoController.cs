
using Microsoft.AspNetCore.Mvc;
using ProjetoEntidades.Models;

namespace DLQ_Projeto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        

        [HttpPost]
        public IActionResult CriarPedido([FromBody] Pedido pedido)
        {
            
            return Ok("Pedido enviado para processamento.");
        }
    }
}