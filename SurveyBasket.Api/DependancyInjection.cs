using MapsterMapper;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SurveyBasket.Api;

public static class  DependancyInjection
{

    public static IServiceCollection AddDependancy ( this IServiceCollection services)
    {

        services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
             services.AddSwaggerServices()
            .AddMapsterServices()
            .AddFluentValidationServices();

       

        return services;
    }

    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
    public static IServiceCollection AddMapsterServices(this IServiceCollection services)
    {

        //Add Mapster

        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));
        return services;
    }
    public static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
    {
        //Add Fluent Validation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();

        return services;
    }
}
