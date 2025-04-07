using api_acesso_ia.Models;

namespace api_acesso_ia.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        //Listar Todos
        Task<IEnumerable<Usuario>> ListarTodos();

        //Buscar dados de um ususario
        Task<Usuario>BuscarPorId(int id);

        //Criar um Usuario
        Task<Usuario> Criar(Usuario dados);

        //Editar um usuario 
        Task<bool> Atualizar(Usuario dados);

        //Excluir Usuario
        Task<bool> Deletar(int id);

        //Valida ja já existe cpf cadastrado
        Task<bool> CpfJaCadastrado(string cpf);


    }
}
