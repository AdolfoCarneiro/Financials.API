namespace Financials.Core.Interfaces;

public interface IUserOwnedResource
{
    Guid UserId { get; set; }
}