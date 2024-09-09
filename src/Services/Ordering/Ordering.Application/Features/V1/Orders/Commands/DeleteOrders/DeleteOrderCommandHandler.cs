using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.V1.Orders.Commands.DeleteOrders;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger _logger;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private const string MethodName = nameof(DeleteOrderCommandHandler);

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"BEGIN: {MethodName} - OrderId: {request.Id}");

        var order = await _orderRepository.GetByIdAsync(request.Id);
        if (order == null)
            throw new NotFoundException(nameof(Order), request.Id);
        await _orderRepository.DeleteAsync(order);
        _orderRepository.SaveChangesAsync();

        _logger.LogInformation($"END: {MethodName} - OrderId: {request.Id}");

        return Unit.Value;
    }
}