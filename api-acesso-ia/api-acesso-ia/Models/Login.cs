using System.ComponentModel.DataAnnotations.Schema;


namespace api_acesso_ia.Models
{
    public class Login
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Logins { get; set; }
        public string Senha { get; set; }
    }
}