using Microsoft.EntityFrameworkCore;
using ShareLink.Application.Common.Abstraction;
using ShareLink.Domain.Models;

namespace ShareLink.Application.Common.Extensions;

public static class ApplicationDbContextExtensions
{
    public static async Task<IReadOnlyCollection<Tag>> CreateTagList(this IApplicationDbContext context, string[] tags, CancellationToken cancellationToken)
    {
        var tagList = new List<Tag>();
        var tagsInDatabase = await context.Tags
            .Where(x => tags.Contains(x.Name))
            .ToArrayAsync(cancellationToken);
        foreach (var requestTag in tags)
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
}