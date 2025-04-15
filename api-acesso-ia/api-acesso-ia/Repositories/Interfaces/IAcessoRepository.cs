using api_acesso_ia.Models;

namespace api_acesso_ia.Repositories.Interfaces
{
    public interface IAcessoRepository
    {
        //Listar Todos
        Task<IEnumerable<AcessoResponse>> ListarTodos();
        Task<bool> Registrar(int IdUsuario, DateTime DataHoraAcesso);
    }
}
