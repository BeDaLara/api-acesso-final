using api_acesso_ia.Models;
using api_acesso_ia.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_acesso_ia.Controllers
{
    [Route("api/v1/usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("listar-todos")]
        public async Task <ActionResult<IList<Usuario>>> ListarTodos()
        {
            var usuarios = await _usuarioService.ListarTodosService();
            return Ok (usuarios);
        }



        [HttpGet("buscar/{id}")]
        public async Task<ActionResult<Usuario>> Buscar(int id)
        {
           var usuario = await _usuarioService.BuscarPorIdService(id);
            if (usuario == null) 
            { 
                return NotFound(new{ msg = "Usuário não encontrado"});
            }
            return Ok(usuario);
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult<Usuario>> Salvar([FromBody] Usuario dados)
        {
            if(await _usuarioService.CpfJaCadastrado(dados.Cpf))
            {
                throw new Exception("O Cpf informado ja possui cadastro");
            }
            var usuario = await _usuarioService.CriarService(dados);
            return CreatedAtAction(nameof(Salvar), new { id = dados.Id },dados);

            }

        [HttpPut("atualizar/{id}")]
        public async Task <IActionResult> Atualizar(int id, [FromBody] Usuario dados)
        {
            if(id != dados.Id) return BadRequest(new {msg = "Dados inválidos"});

            await _usuarioService.AtualizarService(dados);  
            return NoContent();
        }

        [HttpDelete("remover/{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var deletado = await _usuarioService.DeletarService(id);
            if (!deletado)
            {
                return NotFound(new {msg ="Erro ao excluir esse usuário"});
            }
            return NoContent();
        }
        [HttpPost("comparar-imagens")]
        public async Task<IActionResult> CompararImagens([FromBody] ComparacaoImagensDto comparacao)
        {
            try
            {
                var similaridade = await _usuarioService.CompararImagensService(
                    comparacao.Base64Imagem1,
                    comparacao.Base64Imagem2);

                return Ok(new
                {
                    sucesso = true,
                    similaridade,
                    mensagem = similaridade > 0 ?
                        "Imagens comparadas com sucesso" :
                        "Nenhuma similaridade detectada"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    sucesso = false,
                    mensagem = $"Erro ao comparar imagens: {ex.Message}"
                });
            }
        }
    }

    public class ComparacaoImagensDto
    {
        public string Base64Imagem1 { get; set; }
        public string Base64Imagem2 { get; set; }
    }
}

