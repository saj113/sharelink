using MediatR;
using ShareLink.Application.Common.Services;

namespace ShareLink.Application.ToggleLinkLikeHandler;

public class ToggleLinkLikeRequest : IRequest
{
    public string LinkId { get; set; } = string.Empty;

    public bool State { get; init; }
}

public class ToggleLinkLikeHandler(IUserInteractionsService userInteractionsService) : IRequestHandler<ToggleLinkLikeRequest>
{
    public async Task Handle(ToggleLinkLikeRequest request, CancellationToken cancellationToken)
    {
        await userInteractionsService.ToggleLinkLike(request.LinkId, request.State, cancellationToken);
    }
}