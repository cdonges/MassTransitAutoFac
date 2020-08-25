using System;
using System.Threading.Tasks;
using Autofac;
using MassTransit;

namespace MassTransitAutoFac
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var container = CreateContainer();

            var bus = container.Resolve<IBusControl>();

            bus.Start();

            Console.WriteLine("Start");

            for (int i=0; i < 2; i++)
            {
                var id = Guid.NewGuid();
                await bus.Publish(new CategoryCreatedEvent() { Name = i.ToString(), MessageId = id, CorrelationId = Guid.Empty });
            }

            await Task.Run(() => Console.Read());

            bus.Stop();

            Console.WriteLine("Stop");
        }

        public static IContainer CreateContainer()
        {
            var connectionString = Environment.GetEnvironmentVariable("serviceBusTestConnection");

            var builder = new ContainerBuilder();

            builder.RegisterType<CategoryCreatedEventConsumer>().As<IConsumer<CategoryCreatedEvent>>();

            builder.RegisterType<SampleService>().As<ISampleService>();

            var assembly = typeof(CategoryCreatedEventConsumer).Assembly;

            builder.RegisterModule(new MassTransitModule(connectionString, assembly));

            return builder.Build();
        }
    }
}
