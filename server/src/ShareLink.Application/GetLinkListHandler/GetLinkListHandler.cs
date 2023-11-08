﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShareLink.Application.Common.Abstraction;
using ShareLink.Application.Common.Dto;
using ShareLink.Application.Common.Extensions;

namespace ShareLink.Application.GetLinkListHandler;

public class GetLinkListResponse(PaginatedList<LinkDto> paginatedList, TagDto[] tags)
    : PaginatedList<LinkDto>(paginatedList.Items, paginatedList.TotalCount, paginatedList.PageNumber, paginatedList.TotalPages)
{
    public TagDto[] Tags { get; } = tags;
}

public class GetLinkListRequest : IRequest<GetLinkListResponse>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetLinkListHandler(IApplicationDbContext context, IMapper mapper, IIdentityContext identityContext)
    : IRequestHandler<GetLinkListRequest, GetLinkListResponse>
{
    public async Task<GetLinkListResponse> Handle(GetLinkListRequest request, CancellationToken cancellationToken)
    {
        var displayName = identityContext.UserNickname;
        Console.WriteLine($"User {displayName} is requesting links list");
        var links = await context.Links
            .OrderByDescending(x => x.CreatedAt)
            .ProjectTo<LinkDto>(mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);;
        var tags = await context.Tags
            .Include(x => x.Links)
            .Select(x => new TagDto(x.Name, x.Links.Count))
            .ToArrayAsync(cancellationToken);

        return new GetLinkListResponse(links, tags);
    }
}
