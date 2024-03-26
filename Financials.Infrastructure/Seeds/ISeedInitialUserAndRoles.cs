namespace Financials.Infrastructure.Seeds
{
    public interface ISeedInitialUserAndRoles
    {
        void SeedUsers();
        void SeedRoles();
    }
}