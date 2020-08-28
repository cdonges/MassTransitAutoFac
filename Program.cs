using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransitAutoFac
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var container = CreateContainer();

            var bus = container.Resolve<IBus>();
            var globalBus = container.Resolve<IGlobalBus>();
            var busControl = container.Resolve<IBusControl>();

            busControl.Start();

            Console.WriteLine("Start");

            for (int i=0; i < 2; i++)
            {
                var id = Guid.NewGuid();
                var message = new CategoryCreatedEvent() { Name = i.ToString(), MessageId = id, CorrelationId = Guid.NewGuid() };
                Console.WriteLine($"Sending {message.CorrelationId}");
                await bus.Publish(message);
                await globalBus.Publish(message);
            }

            await Task.Run(() => Console.Read());

            busControl.Stop();

            Console.WriteLine("Stop");
        }

        public static IContainer CreateContainer()
        {
            var connectionString = Environment.GetEnvironmentVariable("serviceBusTestConnection");
            var connectionGlobalString = Environment.GetEnvironmentVariable("serviceBusGlobalTestConnection");

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddValdMassTransit(connectionString, connectionGlobalString, "consoleTest", typeof(Program).Assembly);

            var builder = new ContainerBuilder();

            builder.Populate(serviceCollection);

            builder.RegisterType<CategoryCreatedEventConsumer>().As<IConsumer<CategoryCreatedEvent>>();

            builder.RegisterType<SampleService>().As<ISampleService>();

            var assembly = typeof(CategoryCreatedEventConsumer).Assembly;

            return builder.Build();
        }
    }
}
