using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Enums
{
    public enum Roles
    {
        [Display(Name = "Admin")]
        Admin = 0,

        [Display(Name = "Default")]
        Default = 1,

        [Display(Name = "Suporte")]
        Suporte = 2,
    }

    public static class RoleNames
    {
        public const string Admin = "Admin";
        public const string Default = "Default";
        public const string Suporte = "Suporte";
    }
}
