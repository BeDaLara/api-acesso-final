using api_acesso_ia.Models;

namespace api_acesso_ia.Repositories.Interfaces
{
    public interface ILoginRepository
    {

        Task<Login> Autenticar(string login, string senha);

        Task<Login> Cadastrar(Login dados);

        Task<bool> CpfJaCadastrado(string cpf);
        Task<Login> BuscarPorEmail(string email);
        Task<Login> BuscarPorId(int id);
        Task Atualizar(Login dados);
    }
}
