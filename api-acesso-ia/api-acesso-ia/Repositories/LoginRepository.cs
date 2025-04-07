using api_acesso_ia.Models;
using api_acesso_ia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_acesso_ia.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AppDbContext _context;

        public LoginRepository(AppDbContext context)
        {
            _context = context;
        }
        

        public async Task<Login> BuscarPorEmail(string email)
        {
            return await _context.Login.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<Login> BuscarPorId(int id)
        {
            return await _context.Login.FindAsync(id);
        }

        public async Task Atualizar(Login dados)
        {
            _context.Login.Update(dados);
            await _context.SaveChangesAsync();
        }

        public async Task<Login> Autenticar(string login, string senha)
        {
            var user = await _context.Login.FirstOrDefaultAsync(l => l.Logins == login && l.Senha == senha);

            return user != null ? new() { Nome = user.Nome, Email = user.Email, Logins = user.Logins } : null;
        }

        public async Task<Login> Cadastrar(Login dados)
        {
            _context.Login.Add(dados);
            await _context.SaveChangesAsync();
            return dados;
        }

        public async Task<bool> CpfJaCadastrado(string cpf)
        {
            return await _context.Login.AnyAsync(u => u.Cpf == cpf);
        }
    }
}

