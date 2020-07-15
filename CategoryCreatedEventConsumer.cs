namespace MassTransitAutoFac
{
    using System;
    using System.Reflection;
    using Autofac;
    using global::MassTransit;
    using System.Threading.Tasks;

    public class CategoryCreatedEventConsumer : IConsumer<CategoryCreatedEvent>
    {
        public async Task Consume(ConsumeContext<CategoryCreatedEvent> context)
        {
            await Console.Out.WriteLineAsync($"Category Received: {context.Message.Name}");
        }
    }
}