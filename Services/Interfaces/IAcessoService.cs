using api_acesso_ia.Models;
using api_acesso_ia.Requests;

namespace api_acesso_ia.Services.Interfaces
{
    public interface IAcessoService
    {

        //Listar o Historico
        Task<IEnumerable<AcessoResponse>> ListarTodos();
        Task<bool> Registrar(int IdUsuario , DateTime DataHoraAcesso);
    }
}
