﻿using Microsoft.EntityFrameworkCore;
using ShareLink.Domain.Models;

namespace ShareLink.Application.Common.Abstraction;

public interface IApplicationDbContext
{
    DbSet<Link> Links { get; }

    DbSet<Tag> Tags { get; }

    DbSet<User> Users { get; }

    void Push<T>(T entity);

    void Delete<T>(T entity);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}