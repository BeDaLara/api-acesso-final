using api_acesso_ia.Models;

namespace api_acesso_ia.Services.Interfaces
{
    public interface ILoginService
    {
        Task<Login> AutenticarService(string login, string senha);

        Task<Login> CadastrarService(Login dados);
        Task<bool> CpfJaCadastradoService(string cpf);
        string CriptografarSenha(string senha);
        Task<Login> BuscarPorEmailService(string email);
        Task<bool> ResetarSenhaService(int idUsuario);

    }
}