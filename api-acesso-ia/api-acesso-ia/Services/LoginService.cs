using api_acesso_ia.Models;
using api_acesso_ia.Repositories.Interfaces;
using api_acesso_ia.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace api_acesso_ia.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<Login> AutenticarService(string login, string senha)
        {
            var senhaHash = CriptografarSenha(senha);
            return await _loginRepository.Autenticar(login, senhaHash);
        }

        public async Task<Login> CadastrarService(Login dados)
        {
            return await _loginRepository.Cadastrar(dados);
        }

        public async Task<bool> CpfJaCadastradoService(string cpf)
        {
            var possui = await _loginRepository.CpfJaCadastrado(cpf);

            if (possui)
            {
                return true;
            }
            return false;
        }

        public string CriptografarSenha(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public async Task<Login> BuscarPorEmailService(string email)
        {
            return await _loginRepository.BuscarPorEmail(email);
        }

        public async Task<bool> ResetarSenhaService(int idUsuario)
        {
            var usuarioLogin = await _loginRepository.BuscarPorId(idUsuario);
            if (usuarioLogin == null) return false;

            var novaSenha = usuarioLogin.Cpf;
            usuarioLogin.Senha = CriptografarSenha(novaSenha);

            await _loginRepository.Atualizar(usuarioLogin);

            return true;
        }
    }
}