using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace Financials.Core.Entity
{
    [ExcludeFromCodeCoverage]
    public class ApplicationUser : IdentityUser
    {
        public DateTime DataNascimento { get; set; }
        public string Nome { get; set; }

    }
}
