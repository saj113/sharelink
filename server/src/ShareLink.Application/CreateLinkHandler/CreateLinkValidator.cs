﻿using FluentValidation;
using ShareLink.Application.Common.Services;

namespace ShareLink.Application.CreateLinkHandler;

public class CreateLinkValidator : AbstractValidator<CreateLinkRequest>
{
    public CreateLinkValidator(IContentModerator contentModerator)
    {
        RuleFor(request => request)
            .MustAsync((request, _) => contentModerator.ModerateText(request.Title + " " + string.Join(" ", request.Tags)))
            .WithMessage("Title or tags have inappropriate words.");
    }
}