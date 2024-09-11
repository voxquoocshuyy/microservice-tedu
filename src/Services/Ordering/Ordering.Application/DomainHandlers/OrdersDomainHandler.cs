using MediatR;
using Ordering.Domain.OrderAggregate.Events;
using Serilog;

namespace Ordering.Application.DomainHandlers;

public class OrdersDomainHandler :
    INotificationHandler<OrderCreatedEvent>,
    INotificationHandler<OrderDeletedEvent>
{
    private readonly ILogger _logger;

    public OrdersDomainHandler(ILogger logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Ordering Domain Event: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }

    public Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.Information("Ordering Domain Event: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}