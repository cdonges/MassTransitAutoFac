namespace MassTransitAutoFac
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using global::MassTransit;
    using global::MassTransit.MultiBus;
    using Microsoft.Extensions.DependencyInjection;
    using GreenPipes;

    public static class IServiceCollectionExtension
    {
        public static void AddValdMassTransit(
            this IServiceCollection services,
            string massTransitConnectionString,
            string massTransitGlobalConnectionString,
            string apiName,
            params Assembly[] assemblies)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(assemblies);

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(massTransitConnectionString, h => { });

                    cfg.ReceiveEndpoint(apiName, ec =>
                    {
                        ec.ConfigureConsumers(context);
                        ec.DiscardSkippedMessages();
                    });
                });
            });

            if (!string.IsNullOrWhiteSpace(massTransitGlobalConnectionString))
            {
                services.AddMassTransit<IGlobalBus>(g =>
                {
                    g.AddConsumers(assemblies);

                    g.UsingAzureServiceBus((context, cfg) =>
                    {
                        cfg.Host(massTransitGlobalConnectionString, h => { });

                        cfg.ReceiveEndpoint(apiName + "_global", ec =>
                        {
                            ec.ConfigureConsumers(context);
                            ec.DiscardSkippedMessages();
                        });
                    });
                });
            }

            services.AddMassTransitHostedService();
        }
    }
}
