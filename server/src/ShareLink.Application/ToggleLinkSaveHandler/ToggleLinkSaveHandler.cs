using MediatR;
using ShareLink.Application.Common.Abstraction;
using ShareLink.Application.Common.Extensions;

namespace ShareLink.Application.ToggleLinkSaveHandler;

public class ToggleLinkSaveHandler(IApplicationDbContext context, IUserContext userContext) : IRequestHandler<ToggleLinkSaveRequest>
{
    public async Task Handle(ToggleLinkSaveRequest request, CancellationToken cancellationToken)
    {
        var userProfile = await context.GetUserProfile(userContext.UserId, cancellationToken);
        var link = await context.GetLink(request.LinkId, cancellationToken);
        userProfile.ToggleSave(link);

        await context.SaveChangesAsync(cancellationToken);
    }
}