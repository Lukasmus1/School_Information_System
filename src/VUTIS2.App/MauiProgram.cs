﻿using System.Reflection;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using VUTIS2.App.Services;
using VUTIS2.BL;
using VUTIS2.DAL;
using VUTIS2.DAL.Migrator;
using VUTIS2.DAL.Options;

[assembly:System.Resources.NeutralResourcesLanguage("en")]
namespace VUTIS2.App;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        ConfigureAppSettings(builder);

        builder.Services
            .AddDALServices(GetDALOptions(builder.Configuration))
            .AddAppServices()
            .AddBLServices();

        var app = builder.Build();

        MigrateDb(app.Services.GetRequiredService<IDbMigrator>());
        RegisterRouting(app.Services.GetRequiredService<INavigationService>());

        return app;
    }

    private static void ConfigureAppSettings(MauiAppBuilder builder)
    {
        var configurationBuilder = new ConfigurationBuilder();

        var assembly = Assembly.GetExecutingAssembly();
        const string appSettingsFilePath = "VUTIS2.App.appsettings.json";
        using var appSettingsStream = assembly.GetManifestResourceStream(appSettingsFilePath);
        if (appSettingsStream is not null)
        {
            configurationBuilder.AddJsonStream(appSettingsStream);
        }

        var configuration = configurationBuilder.Build();
        builder.Configuration.AddConfiguration(configuration);
    }

    private static void RegisterRouting(INavigationService navigationService)
    {
        foreach (var route in navigationService.Routes)
        {
            Routing.RegisterRoute(route.Route, route.ViewType);
        }
    }

    private static DALOptions GetDALOptions(IConfiguration configuration)
    {
        DALOptions dalOptions = new()
        {
            DatabaseDirectory = FileSystem.AppDataDirectory
        };
        configuration.GetSection("VUTIS2:DAL").Bind(dalOptions);
        return dalOptions;
    }

    private static void MigrateDb(IDbMigrator migrator) => migrator.Migrate();
}
