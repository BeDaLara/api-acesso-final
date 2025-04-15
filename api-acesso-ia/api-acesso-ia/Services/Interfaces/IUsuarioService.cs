using api_acesso_ia.Models;

namespace api_acesso_ia.Services.Interfaces
{
    public interface IUsuarioService
    {
        //Listar Todos
        Task<IEnumerable<Usuario>> ListarTodosService();

        //Buscar dados de um ususario
        Task<Usuario> BuscarPorIdService(int id);

        //Criar um Usuario
        Task<Usuario> CriarService(Usuario dados);

        //Editar um usuario 
        Task<bool> AtualizarService(Usuario dados);

        //Excluir Usuario
        Task<bool> DeletarService(int id);

        Task<bool> CpfJaCadastrado(string cpf);

        Task<bool> EnviarEmailAsync(string destinatario, string assunto, string mensagem);

    }
}
