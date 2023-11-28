using System.ComponentModel.DataAnnotations;
using MediatR;

namespace ShareLink.Application.ToggleLinkSaveHandler;

public class ToggleLinkSaveRequest : IRequest
{
    [Required(ErrorMessage = "Link id is required.")]
    public required string LinkId { get; init; }

    public bool State { get; init; }
}