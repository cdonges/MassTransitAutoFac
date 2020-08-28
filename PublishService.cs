namespace MassTransitAutoFac
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MassTransit;
    using Microsoft.Extensions.Hosting;

    public class PublishService : IHostedService, IDisposable
    {
        private readonly IBus bus;
        private readonly IGlobalBus globalBus;

        public PublishService(IBus bus, IGlobalBus globalBus)
        {
            this.bus = bus;
            this.globalBus = globalBus;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Timed Background Service is starting.");

            await DoWork();
        }

        private async Task DoWork()
        {
            Console.WriteLine("Background send started");

            for (int i=0; i < 2; i++)
            {
                var id = Guid.NewGuid();
                var message = new CategoryCreatedEvent() { Name = i.ToString() + " local", MessageId = id, CorrelationId = Guid.NewGuid() };
                Console.WriteLine($"Sending local {message.CorrelationId}");
                await bus.Publish(message);

                var globalMessage = new CategoryCreatedEvent() { Name = i.ToString() + " global", MessageId = id, CorrelationId = Guid.NewGuid() };
                Console.WriteLine($"Sending global {globalMessage.CorrelationId}");
                await globalBus.Publish(globalMessage);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Background send is stopping.");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}