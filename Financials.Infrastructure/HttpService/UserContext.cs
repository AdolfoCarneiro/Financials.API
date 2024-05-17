using Microsoft.AspNetCore.Http;

namespace Financials.Infrastructure.HttpService;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid GetUserId()
    {
        if (_httpContextAccessor.HttpContext?.Items["UserId"] is Guid userId)
        {
            return userId;
        }
        return Guid.Empty;
    }
}
