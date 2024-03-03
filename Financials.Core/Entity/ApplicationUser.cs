using Microsoft.AspNetCore.Identity;

namespace Financials.Core.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DataNascimento { get; set; }
        public string Nome { get; set; }

    }
}
