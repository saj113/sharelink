﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ShareLink.Migrations.Initializers;

public static class InitializerExtensions
{
    public static async Task RunMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var applicationInitializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        var identityInitializer = scope.ServiceProvider.GetRequiredService<IdentityDbContextInitializer>();

        await Task.WhenAll(applicationInitializer.InitialiseAsync(), identityInitializer.InitialiseAsync());
    }
}
