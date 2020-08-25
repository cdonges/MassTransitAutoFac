namespace MassTransitAutoFac
{
    using System;
    using MassTransit;

    public class CategoryCreatedEvent : CorrelatedBy<Guid>
    {
        public CategoryCreatedEvent()
        {
        }

        public string Name { get; set; }

        public Guid MessageId { get; set; }

        public Guid CorrelationId { get; set; }
    }
}