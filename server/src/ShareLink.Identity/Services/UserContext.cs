using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShareLink.Application.Common.Abstraction;

namespace ShareLink.Identity.Services;

public class UserContext(IHttpContextAccessor contextAccessor) : IUserContext
{
    public string? UserNickname => contextAccessor.HttpContext?.User.FindFirst(ClaimsNames.Nickname)?.Value ??
                                   contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;

    public string? UserId => contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}