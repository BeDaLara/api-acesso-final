using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using api_acesso_ia.Models;
using api_acesso_ia.Repositories.Interfaces;
using api_acesso_ia.Services.Interfaces;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace api_acesso_ia.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private const string AccessKey = "AKIA6D3EDPDGTKSPQ6WD";
        private const string SecretKey = "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";
        private static readonly RegionEndpoint Region = RegionEndpoint.USEast1;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Usuario>> ListarTodosService()
        {
            return await _usuarioRepository.ListarTodos();
        }

        public async Task<Usuario> BuscarPorIdService(int id)
        {
            return await _usuarioRepository.BuscarPorId(id);
        }

        public async Task<Usuario> CriarService(Usuario dados)
        {
            var usuarioCriado = await _usuarioRepository.Criar(dados);

            var assunto = "Bem-vindo ao nosso sistema!";
            var mensagem = $"Bem vindo, {usuarioCriado.Nome}! Seu cadastro foi criado com sucesso!";

            await EnviarEmailAsync(usuarioCriado.Email, assunto, mensagem);

            return usuarioCriado;
        }

        public async Task<bool> AtualizarService(Usuario dados)
        {
            return await _usuarioRepository.Atualizar(dados);
        }

        public async Task<bool> DeletarService(int id)
        {
            return await _usuarioRepository.Deletar(id);
        }

        public async Task<bool> CpfJaCadastrado(string cpf)
        {
            var possui = await _usuarioRepository.CpfJaCadastrado(cpf);
            return possui;
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

        public async Task<float> CompararImagensService(string base64Imagem1, string base64Imagem2)
        {
           
            base64Imagem1 = RemoveBase64Prefix(base64Imagem1);
            base64Imagem2 = RemoveBase64Prefix(base64Imagem2);

            
            var awsClient = new AmazonRekognitionClient(AccessKey, SecretKey, Region);

            
            var request = new CompareFacesRequest
            {
                SourceImage = new Image { Bytes = new MemoryStream(Convert.FromBase64String(base64Imagem1)) },
                TargetImage = new Image { Bytes = new MemoryStream(Convert.FromBase64String(base64Imagem2)) },
                SimilarityThreshold = 70
            };

            try
            {
                var response = await awsClient.CompareFacesAsync(request);

               
                if (response.FaceMatches.Count > 0 && response.FaceMatches[0].Similarity.HasValue)
                {
                    return response.FaceMatches[0].Similarity.Value; 
                }

                Console.WriteLine("Nenhuma similaridade detectada ou valor de similaridade é nulo");
                return 0f; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao comparar imagens: {ex.Message}");
                throw;
            }
        }

        private string RemoveBase64Prefix(string base64String)
        {
            if (base64String.Contains(","))
            {
                return base64String.Split(',')[1];
            }
            return base64String;
        }
    }
}