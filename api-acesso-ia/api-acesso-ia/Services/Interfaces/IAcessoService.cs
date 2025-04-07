using api_acesso_ia.Models;

namespace api_acesso_ia.Services.Interfaces
{
    public interface IAcessoService
    {

        //Listar o Historico
        Task<IEnumerable<AcessoResponse>> ListarTodos();
        Task<bool> Registrar(Acesso acesso);
    }
}
