using api_acesso_ia.Models;
using api_acesso_ia.Repositories.Interfaces;
using api_acesso_ia.Requests;
using api_acesso_ia.Services.Interfaces;
using MailKit.Security;
using MimeKit;

namespace api_acesso_ia.Services
{
    public class AcessoService : IAcessoService
    {
        private readonly IAcessoRepository _acessoRepository;
        private readonly IUsuarioService _usuarioService;

        public AcessoService(IAcessoRepository acessoRepository, IUsuarioService usuarioService)
        {
            _acessoRepository = acessoRepository;
            _usuarioService = usuarioService;
        }
        public Task<IEnumerable<AcessoResponse>> ListarTodos()
        {
            return _acessoRepository.ListarTodos();
        }

        public async Task<bool> Registrar(int IdUsuario, DateTime DataHoraAcesso)
        {
            var resultado = await _acessoRepository.Registrar(IdUsuario, DataHoraAcesso);

            if (resultado)
            {
                try
                {
                    var usuario = await _usuarioService.BuscarPorIdService(IdUsuario);

                    if (usuario != null && !string.IsNullOrWhiteSpace(usuario.Email))
                    {
                        string dataHora = DataHoraAcesso.ToString("dd/MM/yyyy HH:mm:ss");
                        string assunto = "Novo Acesso Registrado";
                        string mensagem = $"Olá {usuario.Nome},\n\nUm novo acesso foi registrado na data: {dataHora}.\n\nSe não foi você, por favor entre em contato com o suporte.";

                        await _usuarioService.EnviarEmailAsync(usuario.Email, assunto, mensagem);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                }
            }

            return resultado;
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
