namespace MassTransitAutoFac
{
    using System;

    public class CategoryCreatedEvent
    {
        public CategoryCreatedEvent()
        {
        }

        public string Name { get; set; }

        public Guid MessageId { get; set; }
    }
}