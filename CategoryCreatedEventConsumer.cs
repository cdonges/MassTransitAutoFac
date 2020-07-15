namespace MassTransitAutoFac
{
    using System;
    using System.Reflection;
    using Autofac;
    using global::MassTransit;
    using System.Threading.Tasks;

    public class CategoryCreatedEventConsumer : IConsumer<CategoryCreatedEvent>
    {
        private readonly ISampleService sampleService;

        public CategoryCreatedEventConsumer(ISampleService sampleService)
        {
            this.sampleService = sampleService;
        }

        public async Task Consume(ConsumeContext<CategoryCreatedEvent> context)
        {
            var x = this.sampleService.AddNumbers(1, 2);
            await Console.Out.WriteLineAsync($"Category Received: {context.Message.Name}");
        }
    }
}