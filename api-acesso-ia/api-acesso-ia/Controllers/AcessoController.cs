using api_acesso_ia.Models;
using api_acesso_ia.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_acesso_ia.Controllers
{
    [Route("api/v1/historico")]
    [ApiController]
    public class AcessoController : ControllerBase
    {
        private readonly IAcessoService _acessoService;

        public AcessoController(IAcessoService acessoService)
        {
            _acessoService = acessoService;
        }
        [HttpGet("listar-todos")]
        public async Task<IActionResult> Listar()
        {
            var acessos = await _acessoService.ListarTodos();
            return Ok(acessos);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] Acesso dados)
        {
            var resultado = await _acessoService.Registrar(dados);
            if (!resultado)
                return BadRequest("Erro ao registrar acesso");

            return Ok("Registro de acesso salvo com sucesso.");
        }
    }
}
