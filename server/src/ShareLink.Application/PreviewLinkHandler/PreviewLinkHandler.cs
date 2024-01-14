﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using ShareLink.Application.Common.Abstraction;
using ShareLink.Application.Common.Dto;
using ShareLink.Application.Common.Services;
using ShareLink.Domain;
using ShareLink.Domain.Enums;

namespace ShareLink.Application.PreviewLinkHandler;

public class PreviewLinkHandler(IUrlParser urlParser, IGoogleApiService googleApiService, IApplicationDbContext context)
    : IRequestHandler<PreviewLinkRequest, PreviewLinkResponse>
{
    public Task<PreviewLinkResponse> Handle(PreviewLinkRequest request, CancellationToken cancellationToken)
    {
        var (linkType, id) = urlParser.ParseUrl(request.Url);
        return linkType switch
        {
            LinkType.Youtube => HandleYoutube(id),
            LinkType.UnknownSource => Task.FromResult(HandleUnknownSourceLink(id)),
            _ => throw new NotSupportedException($"Link type {linkType} is not supported.")
        };
    }

    private async Task<PreviewLinkResponse> HandleYoutube(string id)
    {
        var videoInfo = await googleApiService.GetYoutubeVideoInfo(id);
        var tags = await FindMostMentionedTags(videoInfo.Tags);
        var title = videoInfo.Title.Length > ValidationRules.LinkTitle.MaxLength
            ? videoInfo.Title[..(ValidationRules.LinkTitle.MaxLength - 3)] + "..."
            : videoInfo.Title;
        return new PreviewLinkResponse
        {
            Title = title,
            Type = LinkType.Youtube,
            Youtube = new YoutubeDataDto
            {
                VideoId = videoInfo.Id
            },
            Tags = tags,
        };
    }

    private async Task<string[]> FindMostMentionedTags(IEnumerable<string> youtubeVideoTags)
    {
        var allTags = await context.Tags.Select(tag => tag.Name).ToArrayAsync();
        var tagOccurrences = new Dictionary<string, int>();

        foreach (var videoTag in youtubeVideoTags.Distinct())
        {
            var lowerCaseVideoTag = videoTag.ToLowerInvariant();
            var matchedTags = allTags.Where(tag => lowerCaseVideoTag.Contains(tag));
            foreach (var matchedTag in matchedTags)
            {
                tagOccurrences.TryGetValue(matchedTag, out var value);
                tagOccurrences[matchedTag] = ++value;
            }
        }

        return tagOccurrences.OrderByDescending(kv => kv.Value).Take(ValidationRules.Tag.MaxSuggestedTagsCount).Select(kv => kv.Key).ToArray();
    }

    private PreviewLinkResponse HandleUnknownSourceLink(string id) =>
        new()
        {
            Title = "",
            Type = LinkType.UnknownSource,
            UnknownSource = new UnknownSourceDataDto { Url = id },
            Tags = Array.Empty<string>(),
        };
}