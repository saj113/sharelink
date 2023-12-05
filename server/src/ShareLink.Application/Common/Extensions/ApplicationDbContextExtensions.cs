using Microsoft.EntityFrameworkCore;
using ShareLink.Application.Common.Abstraction;
using ShareLink.Application.Common.Exceptions;
using ShareLink.Common.Exceptions;
using ShareLink.Domain.Models;

namespace ShareLink.Application.Common.Extensions;

public static class ApplicationDbContextExtensions
{
    public static async Task<IReadOnlyCollection<Tag>> CreateTagList(this IApplicationDbContext context, string[] tags, CancellationToken cancellationToken)
    {
        var lowerCaseTags = tags.Select(x => x.ToLower()).Distinct().ToArray();
        var tagList = new List<Tag>();
        var tagsInDatabase = await context.Tags
            .Where(x => lowerCaseTags.Contains(x.Name))
            .ToArrayAsync(cancellationToken);
        foreach (var requestTag in lowerCaseTags)
        {
            var tag = tagsInDatabase.FirstOrDefault(x => x.Name == requestTag);
            if (tag is null)
            {
                tag = Tag.Create(requestTag);
                context.Tags.Add(tag);
            }

            tagList.Add(tag);
        }

        return tagList;
    }

    public static async Task<Link> GetLink(this IApplicationDbContext context, string linkId, CancellationToken cancellationToken)
    {
        var link = await context.Links.SingleOrDefaultAsync(x => x.Id == linkId, cancellationToken);
        if (link == null)
        {
            throw new BusinessException(ErrorCodes.LinkNotFound);
        }

        return link;
    }

    public static async Task<UserProfile> GetUserProfile(this IApplicationDbContext context, string? userId, CancellationToken cancellationToken)
    {
        if (userId == null)
        {
            throw new UserUnauthorizedException();
        }

        var userProfile = await context.UserProfiles
            .Include(x => x.LikedLinks)
            .Include(x => x.DislikedLinks)
            .Include(x => x.SavedLinks)
            .SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (userProfile == null)
        {
            userProfile = new UserProfile
            {
                UserId = userId
            };
            context.Push(userProfile);
        }

        return userProfile;
    }
}