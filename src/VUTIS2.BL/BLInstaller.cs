using Microsoft.Extensions.DependencyInjection;
using VUTIS2.BL.Facades;
using VUTIS2.BL.Mappers;
using VUTIS2.DAL.UnitOfWork;

namespace VUTIS2.BL;

public static class BLInstaller
{
    public static IServiceCollection AddBLServices(this IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
        services.Scan(selector => selector
            .FromAssemblyOf<BusinessLogic>()
            .AddClasses(filter => filter.AssignableTo(typeof(IFacade<,,>)))
            .AsMatchingInterface()
            .WithSingletonLifetime());

        services.Scan(selector => selector
            .FromAssemblyOf<BusinessLogic>()
            .AddClasses(filter => filter.AssignableTo(typeof(IModelMapper<,,>)))
            .AsSelfWithInterfaces()
            .WithSingletonLifetime());
        services.AddSingleton<IServiceProvider>(provider => provider);
        return services;
    }
}
