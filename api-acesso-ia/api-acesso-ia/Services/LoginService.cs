using api_acesso_ia.Models;
using api_acesso_ia.Repositories.Interfaces;
using api_acesso_ia.Services.Interfaces;
using MailKit.Security;
using MimeKit;
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

        public async Task<bool> ResetarSenhaService(int idUsuario, string novaSenha)
        {
            var usuarioLogin = await _loginRepository.BuscarPorId(idUsuario);
            if (usuarioLogin == null) return false;

            usuarioLogin.Senha = CriptografarSenha(novaSenha);

            await _loginRepository.Atualizar(usuarioLogin);

            await this.EnviarEmailAsync(usuarioLogin.Email,
                    "Sua senha foi resetada",
                    $"Olá {usuarioLogin.Nome}, sua senha foi redefinida com sucesso!");

            return true;
        }
            public async Task<bool> EnviarEmailAsync(string destinatario, string assunto, string mensagem)
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Meu sistema", "bernardosilvacorlaite2406@gmail.com"));
                email.To.Add(MailboxAddress.Parse(destinatario));
                email.Subject = assunto;

                email.Body = new TextPart("plain") { Text = mensagem };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                try
                {
                    await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync("bernardosilvacorlaite2406@gmail.com", "zejq bwqi smtl zyqj"); 
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar email: {ex.Message}");
                    return false;
                }
            }
    }
}