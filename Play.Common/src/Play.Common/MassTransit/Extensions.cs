using System;
using System.Reflection;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;

namespace Play.Common.MassTransit;

// Commands to publish the lib as a package on the packages folder
// e.g. dotnet pack -p:PackageVersion=1.0.1 -o ..\..\..\packages\

public static class Extensions
{
    public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
    {
        services.AddMassTransit(configure =>
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());

                configure.UsingRabbitMq((context, configurator) =>
                {
                    var configuration = context.GetService<IConfiguration>();
                    var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                    var RabbitMQSettings = configuration.GetSection(nameof(Settings.RabbitMQSettings)).Get<RabbitMQSettings>();
                    configurator.Host(RabbitMQSettings.Host);
                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(prefix: serviceSettings.ServiceName, includeNamespace: false));
                    configurator.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(3, TimeSpan.FromSeconds(5));
                    });
                });
            });

        services.AddMassTransitHostedService();

        return services;
    }
}