using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MassTransitAutoFac
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    CreateContainer(builder);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<PublishService>();
                })
                .RunConsoleAsync();
        }

        public static void CreateContainer(ContainerBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("serviceBusTestConnection");
            var connectionGlobalString = Environment.GetEnvironmentVariable("serviceBusGlobalTestConnection");

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddValdMassTransit(connectionString, connectionGlobalString, "consoleTest", typeof(Program).Assembly);

            builder.Populate(serviceCollection);

            builder.RegisterType<CategoryCreatedEventConsumer>().As<IConsumer<CategoryCreatedEvent>>();

            builder.RegisterType<SampleService>().As<ISampleService>();
        }
    }
}
