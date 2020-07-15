namespace MassTransitAutoFac
{
    using System;
    using System.Reflection;
    using Autofac;
    using global::MassTransit;
    using global::MassTransit.Azure;
    using global::MassTransit.AutofacIntegration;

    public class MassTransitModule : Autofac.Module
    {
        /// <summary>
        /// Assemblies to be scanned for Mass transit Handlers.
        /// </summary>
        private readonly Assembly[] assemblies;

        /// <summary>
        /// The connection string to be used with Asure.
        /// </summary>
        private readonly string massTransitConnectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitModule"/> class.
        /// </summary>
        /// <param name="connectionString">The Azure Connection String.</param>
        /// <param name="assemblies">Assemblies to scan for Mass Transit Commands and Event Handlers.</param>
        public MassTransitModule(
            string massTransitConnectionString,
            params Assembly[] assemblies)
        {
            this.assemblies = assemblies;
            this.massTransitConnectionString = massTransitConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.AddMassTransit(x =>
            {
                x.AddConsumers(this.assemblies);

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(this.massTransitConnectionString,
                             h => { });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
