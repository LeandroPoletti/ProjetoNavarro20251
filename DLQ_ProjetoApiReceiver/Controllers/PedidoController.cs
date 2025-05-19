
using Microsoft.AspNetCore.Mvc;
using Pedido.Infra;
using Pedido.Infra.Interfaces;


namespace DLQ_Projeto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IRepositorio _repositorio;

        public PedidosController(IRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpPost]
        public IActionResult CriarPedido([FromBody] ProjetoEntidades.Models.Pedido pedido)
        {
            var res = _repositorio.Adicionar<ProjetoEntidades.Models.Pedido>(pedido);
            
            return res > 0 ? Ok("Pedido enviado para processamento.") :  BadRequest("ERRO");
        }
    }
}