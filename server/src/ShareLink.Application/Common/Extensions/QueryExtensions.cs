﻿using Microsoft.EntityFrameworkCore;
using ShareLink.Application.Common.Dto;

namespace ShareLink.Application.Common.Extensions;

public static class QueryExtensions
{
    public static async Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNumber,
        int pageSize) where TDestination : class
    {
        var count = await queryable.CountAsync();
        var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return PaginatedList<TDestination>.Create(items, count, pageNumber, pageSize);
    }
}
